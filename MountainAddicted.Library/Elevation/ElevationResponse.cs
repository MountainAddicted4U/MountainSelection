using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MountainAddicted.Library.Elevation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ElevationResponse : IServiceResponse
    {
        /// <summary>
        /// Contains the ServiceResponseStatus.
        /// </summary>
        [JsonProperty("status")]
        public ServiceResponseStatus Status { get; set; }

        /// <summary>
        /// More detailed information about the reasons behind the given status code, if other than OK.
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("results")]
        public ElevationResult[] Results { get; set; }
    }
}
