using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Elevation
{
    /// <summary>
    /// Provides a request for the Google Maps Elevation web service.
    /// </summary>
    public class ElevationRequest : BaseRequest
    {
        /// <summary>
        /// Defines the location(s) on the earth from which to return elevation
        /// data. This parameter takes either a single location as a
        /// comma-separated {latitude,longitude} pair (e.g. "40.714728,-73.998672")
        /// or multiple latitude/longitude pairs passed as an array or as an
        /// encoded polyline.
        /// </summary>
        /// <remarks>Required if path not present.</remarks>
        /// <see href="http://code.google.com/apis/maps/documentation/elevation/#Locations"/>
        public IList<LatLng> Locations
        {
            get
            {
                if (_locations == null) _locations = new List<LatLng>();
                return _locations;
            }
        }
        private IList<LatLng> _locations;

        /// <summary>
        /// Easy way to add locations to the Locations collection.
        /// </summary>
        /// <param name="locationCollection"></param>
        public void AddLocations(params LatLng[] locationCollection)
        {
            if (locationCollection == null) return;

            IList<LatLng> myLocations = this.Locations;
            foreach (LatLng item in locationCollection)
            {
                this.Locations.Add(item);
            }
        }

        /// <summary>
        /// Defines a path on the earth for which to return elevation data.
        /// This parameter defines a set of two or more ordered {latitude,
        /// longitude} pairs defining a path along the surface of the earth.
        /// This parameter must be used in conjunction with the samples
        /// parameter.
        /// </summary>
        /// <remarks>Required if locations not present.</remarks>
        /// <see href="http://code.google.com/apis/maps/documentation/elevation/#Paths"/>
        public IList<LatLng> Path
        {
            get
            {
                if (this._path == null) this._path = new List<LatLng>();
                return this._path;
            }
        }
        private IList<LatLng> _path;

        /// <summary>
        /// specifies the number of sample points along a path for which to return
        /// elevation data. The samples parameter divides the given path into an
        /// ordered set of equidistant points along the path.
        /// </summary>
        /// <remarks>Required if a path is specified.</remarks>
        public int? Samples { get; set; }

        public override Uri ToUri()
        {
            var qsb = new   QueryStringBuilder()

                .Append("locations", RequestUtils.GetLatLngCollectionStr(this._locations))
                .Append("path", RequestUtils.GetLatLngCollectionStr(this._path))
                .Append("samples", (Samples.GetValueOrDefault() > 0 ? Samples.ToString() : ""));

            var url = "json?" + qsb.ToString();

            return new Uri(url, UriKind.Relative);
        }

        private string GetLocationsStr()
        {
            return null;
        }

        private string GetPathStr()
        {
            return null;
        }
    }
}