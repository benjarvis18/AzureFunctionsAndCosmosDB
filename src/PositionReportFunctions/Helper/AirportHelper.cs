using System;
using System.Collections.Generic;
using System.Text;

namespace PositionReportFunctions.Helper
{
    public static class AirportHelper
    {
        public static string GetIcaoCode(string airport)
        {
            if (string.IsNullOrWhiteSpace(airport))
            {
                return null;
            }

            return airport.Substring(0, 4);
        }

        public static string GetAirportName(string airport)
        {
            if (string.IsNullOrWhiteSpace(airport))
            {
                return null;
            }

            return airport.Substring(5);
        }
    }
}
