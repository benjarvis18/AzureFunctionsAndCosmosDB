using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PositionReportFunctions.Helper
{
    public static class DocumentClientInstance
    {
        private static DocumentClient _documentClient = null;
        private static object _lock = new object();

        public static DocumentClient GetDocumentClient()
        {
            if (_documentClient != null)
            {
                return _documentClient;
            }

            var cosmosDbAccount = CosmosDbAccount.Parse(Environment.GetEnvironmentVariable("AircraftPositionReportsCosmosDB"));

            lock (_lock)
            {
                if (_documentClient == null)
                {
                    _documentClient = new DocumentClient(new Uri(cosmosDbAccount.Endpoint), cosmosDbAccount.Key);
                }
            }

            return _documentClient;
        }
    }
}
