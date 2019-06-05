using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Spatial;

namespace AzureFunctionsAndCosmosDB.Domain.Models
{
    public class Aircraft
    {
        [JsonProperty("id")]
        public string Id => TransponderIdentifier;

        [JsonProperty("ttl")]
        public int TimeToLive { get; set; }

        [JsonProperty("transponderIdentifier")]
        public string TransponderIdentifier { get; set; }

        [JsonProperty("registration")]
        public string Registration { get; set; }

        [JsonProperty("aircraftType")]
        public string AircraftType { get; set; }

        [JsonProperty("flightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("departureAirport")]
        public string DepartureAirport { get; set; }

        [JsonProperty("departureIcao")]
        public string DepartureIcao { get; set; }

        [JsonProperty("arrivalAirport")]
        public string ArrivalAirport { get; set; }

        [JsonProperty("arrivalIcao")]
        public string ArrivalIcao { get; set; }

        [JsonProperty("currentAltitude")]
        public int CurrentAltitude { get; set; }

        [JsonProperty("currentVerticalSpeed")]
        public int CurrentVerticalSpeed { get; set; }

        [JsonProperty("currentLatitude")]
        public double CurrentLatitude { get; set; }

        [JsonProperty("currentLongitude")]
        public double CurrentLongitude { get; set; }

        [JsonProperty("currentTrueTrackDegrees")]
        public decimal CurrentTrueTrackDegrees { get; set; }

        [JsonProperty("distanceFromUs")]
        public decimal DistanceFromUs { get; set; }
    }
}
