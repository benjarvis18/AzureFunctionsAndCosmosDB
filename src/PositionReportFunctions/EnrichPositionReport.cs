using AzureFunctionsAndCosmosDB.Domain.Models;
using GeoCoordinatePortable;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PositionReportFunctions.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PositionReportFunctions
{
    public static class EnrichPositionReport
    {
        private const string SOURCE_COLLECTION_NAME = "PositionReports";
        private const string DESTINATION_COLLECTION_NAME = "Aircraft";
        private const string DATABASE_NAME = "AircraftPositionReports";

        private const decimal METERS_TO_FEET_CONVERSION_FACTOR = 3.281M;

        [FunctionName("EnrichPositionReport")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: DATABASE_NAME,
            collectionName: SOURCE_COLLECTION_NAME,
            ConnectionStringSetting = "AircraftPositionReportsCosmosDB",
            LeaseCollectionName = "Leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            log.LogInformation($"{input.Count} documents received.");

            foreach (var document in input)
            {
                var documentClient = DocumentClientInstance.GetDocumentClient();
                var positionReport = JsonConvert.DeserializeObject<PositionReport>(document.ToString());

                log.LogInformation($"Processing position report for {positionReport.TransponderIdentifier}");

                Aircraft aircraft = null;

                var isNew = false;

                // Get from Cosmos DB or create a new record if one doesn't exist
                try
                {
                    var response = await documentClient.ReadDocumentAsync<Aircraft>(
                        UriFactory.CreateDocumentUri(DATABASE_NAME, DESTINATION_COLLECTION_NAME, positionReport.TransponderIdentifier),
                        new RequestOptions()
                        {
                            PartitionKey = new PartitionKey(positionReport.TransponderIdentifier)
                        });

                    aircraft = response.Document;
                }
                catch (DocumentClientException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        aircraft = new Aircraft()
                        {
                            TransponderIdentifier = positionReport.TransponderIdentifier
                        };

                        isNew = true;
                    }
                    else
                    {
                        throw;
                    }
                }

                aircraft.Registration = positionReport.Callsign.Trim();
                aircraft.TimeToLive = 86400;

                // Extract details from the position report
                if (positionReport.Latitude != aircraft.CurrentLatitude || positionReport.Longitude != aircraft.CurrentLongitude)
                {
                    aircraft.CurrentAltitude = decimal.ToInt32(positionReport.AltitudeInMeters * METERS_TO_FEET_CONVERSION_FACTOR);
                    aircraft.CurrentVerticalSpeed = decimal.ToInt32(positionReport.VerticalRateInMetersPerSecond * METERS_TO_FEET_CONVERSION_FACTOR);

                    aircraft.CurrentLatitude = positionReport.Latitude;
                    aircraft.CurrentLongitude = positionReport.Longitude;

                    aircraft.CurrentTrueTrackDegrees = positionReport.TrueTrackDegrees;

                    aircraft.DistanceFromUs = (decimal)Math.Round(CalculateDistanceRemaining(aircraft.CurrentLatitude, aircraft.CurrentLongitude), 2);

                    if (isNew)
                    {
                        aircraft.FlightNumber = positionReport.Callsign;
                    }

                    // Upsert the document
                    await documentClient.UpsertDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(DATABASE_NAME, DESTINATION_COLLECTION_NAME),
                        aircraft,
                        new RequestOptions()
                        {
                            PartitionKey = new PartitionKey(aircraft.TransponderIdentifier)
                        });
                }
            }
        }

        private static double CalculateDistanceRemaining(double currentLat, double currentLong)
        {
            var currentCoordinate = new GeoCoordinate(currentLat, currentLong);
            var destinationCoordinate = new GeoCoordinate(50.961349, -1.422748);

            return currentCoordinate.GetDistanceTo(destinationCoordinate) / 1852;
        }
    }
}