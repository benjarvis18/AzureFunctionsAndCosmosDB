using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsAndCosmosDB.Domain.Models
{
    /// <summary>
    /// The list of aircraft that is sent to the browser as a JSON file.
    /// </summary>
    [DataContract]
    public class AircraftList
    {
        /// <summary>
        /// Gets the list of aircraft to show to the user.
        /// </summary>
        public List<PositionReport> Aircraft { get; private set; }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        public AircraftList()
        {
            Aircraft = new List<PositionReport>();
        }
    }
}
