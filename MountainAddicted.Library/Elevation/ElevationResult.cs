using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MountainAddicted.Library.Elevation
{ 
    [JsonObject(MemberSerialization.OptIn)]
    public class ElevationResult
    {
        [JsonProperty("location")]
        public LatLng Location { get; set; }

        [JsonProperty("elevation")]
        public decimal Elevation { get; set; }

        [JsonProperty("resolution")]
        public decimal Resolution { get; set; }
    }
}