using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsAndCosmosDB.Domain.Models
{
    public class PositionReport
    {
        [JsonProperty("transponderIdentifier")]
        public string TransponderIdentifier { get; set; }

        [JsonProperty("callsign")]
        public string Callsign { get; set; }

        [JsonProperty("originCountry")]
        public string OriginCountry { get; set; }

        [JsonProperty("timePosition")]
        public int TimePosition { get; set; }

        [JsonProperty("lastContact")]
        public int LastContact { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("altitudeInMeters")]
        public decimal AltitudeInMeters { get; set; }

        [JsonProperty("onGround")]
        public bool OnGround { get; set; }

        [JsonProperty("velocityInMetersPerSecond")]
        public decimal VelocityInMetersPerSecond { get; set; }

        [JsonProperty("trueTrackDegrees")]
        public decimal TrueTrackDegrees { get; set; }

        [JsonProperty("verticalRateInMetersPerSecond")]
        public decimal VerticalRateInMetersPerSecond { get; set; }

        [JsonProperty("geoAltitudeInMeters")]
        public decimal GeoAltitudeInMeters { get; set; }

        [JsonProperty("squawk")]
        public string Squawk { get; set; }
    }
}
