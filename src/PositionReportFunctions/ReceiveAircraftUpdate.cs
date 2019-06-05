using System.Collections.Generic;
using System.Threading.Tasks;
using AzureFunctionsAndCosmosDB.Domain.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PositionReportFunctions
{
    public static class ReceiveAircraftUpdate
    {
        [FunctionName("ReceiveAircraftUpdate")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "AircraftPositionReports",
            collectionName: "Aircraft",
            ConnectionStringSetting = "AircraftPositionReportsCosmosDB",
            LeaseCollectionName = "Leases")]IReadOnlyList<Document> input,
            [SignalR(HubName = "aircraft")]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation($"{input.Count} documents received.");

            foreach (var document in input)
            {
                var aircraft = JsonConvert.DeserializeObject<Aircraft>(document.ToString());

                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "newMessage",
                        Arguments = new[] { aircraft }
                    });
            }
        }
    }
}
