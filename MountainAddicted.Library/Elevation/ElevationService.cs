using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Elevation
{
    /// <summary>
    /// The Elevation service provides elevation data for all locations on the surface of the earth, including depth locations on the ocean floor (which return negative values).
    /// In those cases where Google does not possess exact elevation measurements at the precise location you request, the service will interpolate and return an averaged value
    /// using the four nearest locations.
    /// </summary>
    /// <see href="http://code.google.com/apis/maps/documentation/elevation/"/>
    public class ElevationService : IDisposable
    {
        public static readonly Uri HttpsUri = new Uri("https://maps.google.com/maps/api/elevation/");
        public static readonly Uri HttpUri = new Uri("http://maps.google.com/maps/api/elevation/");

        Uri baseUri;
        MapsHttp http;

        public ElevationService(GoogleSigned signingSvc = null, Uri baseUri = null)
        {
            this.baseUri = baseUri ?? HttpsUri;

            this.http = new MapsHttp(signingSvc ?? GoogleSigned.SigningInstance);
        }

        /// <summary>
        /// Sends the specified request to the Google Maps Elevation web
        /// service and parses the response as an ElevationResponse
        /// object.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ElevationResponse GetResponse(ElevationRequest request)
        {
            var url = new Uri(baseUri, request.ToUri());

            return http.Get<ElevationResponse>(url);
        }

        public async Task<ElevationResponse> GetResponseAsync(ElevationRequest request)
        {
            var url = new Uri(baseUri, request.ToUri());

            return await http.GetAsync<ElevationResponse>(url);
        }

        public void Dispose()
        {
            if (http != null)
            {
                http.Dispose();
                http = null;
            }
        }
    }
}
