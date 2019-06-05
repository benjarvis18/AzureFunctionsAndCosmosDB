using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PositionReportFunctions
{
    public static class ReceivePositionReport
    {
        [FunctionName("ReceivePositionReport")]
        public static void Run(
            [EventHubTrigger("positionreports", Connection = "AircraftPositionReportsEventHub")]string message,
            [CosmosDB(
                databaseName: "AircraftPositionReports",
                collectionName: "PositionReports",
                ConnectionStringSetting = "AircraftPositionReportsCosmosDB")]out dynamic document,
            ILogger log)
        {
            log.LogInformation($"Posting position report to Cosmos DB");

            document = message;
        }
    }
}
