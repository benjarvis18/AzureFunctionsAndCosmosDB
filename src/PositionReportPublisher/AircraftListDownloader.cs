using AzureFunctionsAndCosmosDB.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PositionReportPublisher
{
    public class AircraftListDownloader
    {
        private const string WEB_ADDRESS = "https://opensky-network.org/api/states/all?lamin=50.38297&lomin=-2.40300&lamax=51.73655&lomax=0.91612";

        public async Task<AircraftList> DownloadAircraftList()
        {            
            var downloadAddress = WEB_ADDRESS;
            
            using (var webClient = new WebClient())
            {
                var response = await webClient.DownloadStringTaskAsync(downloadAddress).ConfigureAwait(false);
                dynamic responseData = JObject.Parse(response);

                var aircraftList = new AircraftList();

                foreach (var report in responseData["states"])
                {
                    var positionReport = new PositionReport();

                    positionReport.TransponderIdentifier = report[0].ToString();
                    positionReport.Callsign = report[1].ToString();
                    positionReport.OriginCountry = report[2].ToString();

                    if (int.TryParse(report[3].ToString(), out int timePosition))
                    {
                        positionReport.TimePosition = timePosition;
                    }

                    if (int.TryParse(report[4].ToString(), out int lastContact))
                    {
                        positionReport.LastContact = lastContact;
                    }

                    if (double.TryParse(report[5].ToString(), out double longitude))
                    {
                        positionReport.Longitude = longitude;
                    }

                    if (double.TryParse(report[6].ToString(), out double latitude))
                    {
                        positionReport.Latitude = latitude;
                    }

                    if (decimal.TryParse(report[7].ToString(), out decimal altitudeInMeters))
                    {
                        positionReport.AltitudeInMeters = altitudeInMeters;
                    }

                    if (bool.TryParse(report[8].ToString(), out bool onGround))
                    {
                        positionReport.OnGround = onGround;
                    }

                    if (decimal.TryParse(report[9].ToString(), out decimal velocityInMetersPerSecond))
                    {
                        positionReport.VelocityInMetersPerSecond = velocityInMetersPerSecond;
                    }

                    if (decimal.TryParse(report[10].ToString(), out decimal trueTrackDegrees))
                    {
                        positionReport.TrueTrackDegrees = trueTrackDegrees;
                    }

                    if (decimal.TryParse(report[11].ToString(), out decimal verticalRateInMetersPerSecond))
                    {
                        positionReport.VerticalRateInMetersPerSecond = verticalRateInMetersPerSecond;
                    }

                    if (decimal.TryParse(report[13].ToString(), out decimal geoAltitudeInMeters))
                    {
                        positionReport.GeoAltitudeInMeters = geoAltitudeInMeters;
                    }

                    positionReport.Squawk = report[14].ToString();

                    aircraftList.Aircraft.Add(positionReport);
                }

                return aircraftList;
            }
        }
    }
}
