using System;
using System.Collections.Generic;
using System.Text;

namespace PositionReportFunctions.Helper
{
    public class CosmosDbAccount
    {
        private const string ENDPOINT_KEY = "AccountEndpoint";
        private const string KEY_KEY = "AccountKey";
        private const string DATABASE_KEY = "Database";

        public string Endpoint { get; }

        public string Key { get; }

        public string Database { get; }

        public CosmosDbAccount(string endpoint, string key)
            : this(endpoint, key, null)
        { }

        public CosmosDbAccount(string endpoint, string key, string database)
        {
            Endpoint = endpoint;
            Key = key;
            Database = database;
        }

        public static CosmosDbAccount Parse(string connectionString)
        {
            var settings = ParseStringIntoSettings(connectionString);

            settings.TryGetValue(DATABASE_KEY, out string databaseName);

            return new CosmosDbAccount(settings[ENDPOINT_KEY], settings[KEY_KEY], databaseName);
        }

        private static IDictionary<string, string> ParseStringIntoSettings(string connectionString)
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();
            string[] split = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string nameValue in split)
            {
                string[] splitNameValue = nameValue.Split(new char[] { '=' }, 2);

                if (splitNameValue.Length != 2)
                {
                    throw new InvalidOperationException("Settings must be of the form \"name=value\".");
                }

                if (settings.ContainsKey(splitNameValue[0]))
                {
                    throw new InvalidOperationException($"Duplicate setting '{splitNameValue[0]}' found.");
                }

                settings.Add(splitNameValue[0].Trim(), splitNameValue[1].Trim());
            }

            return settings;
        }
    }
}
