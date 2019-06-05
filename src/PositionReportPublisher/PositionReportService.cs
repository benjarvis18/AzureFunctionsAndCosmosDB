using AzureFunctionsAndCosmosDB.Domain.Models;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionReportPublisher
{
    public class PositionReportService
    {
        private static readonly AircraftListDownloader _aircraftListDownloader = new AircraftListDownloader();

        private const string EVENT_HUB_NAME = "positionreports";
        private const string EVENT_HUB_CONNECTION_STRING = "";

        public async Task CollectAndPublishPositionReports()
        {
            var aircraftList = await _aircraftListDownloader.DownloadAircraftList();

            var eventHubClient = EventHubClient.CreateFromConnectionString(EVENT_HUB_CONNECTION_STRING, EVENT_HUB_NAME);

            await Task.WhenAll(
                from partition in Partitioner
                    .Create(aircraftList.Aircraft, true)
                    .GetPartitions(5)
                select Task.Run(async delegate
                {
                    using (partition)
                    {
                        while (partition.MoveNext())
                        {
                            var aircraft = partition.Current;
                            
                            await eventHubClient.SendAsync(
                                new EventData(
                                    Encoding.UTF8.GetBytes(
                                        JsonConvert.SerializeObject(
                                            aircraft,
                                            Formatting.Indented,
                                            new JsonSerializerSettings
                                            {
                                                NullValueHandling = NullValueHandling.Ignore
                                            }
                                        )
                                    )
                                )
                                { PartitionKey = aircraft.TransponderIdentifier });

                            Console.WriteLine($"Published Position Report for {aircraft.Callsign}");
                        }
                    }
                }));
        }        
    }
}
