using MountainAddicted.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using System.Threading.Tasks;
using MountainAddicted.Library;
using MountainAddicted.Library.Elevation;
using System.Threading;
using System.Web.Configuration;

namespace MountainAddicted
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string GoogleApiKey { get; set; }

        private Thread heightRequestThreadReference = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GoogleApiKey = WebConfigurationManager.AppSettings["GoogleMapsApiKey"];
            GoogleSigned.AssignAllServices(new GoogleSigned(GoogleApiKey));

            heightRequestThreadReference = 
                new Thread(new ThreadStart(new HeightRequestProcessor().Process));
            heightRequestThreadReference.IsBackground = true;
            heightRequestThreadReference.Start();
        }

        protected void Application_End()
        {
            if(heightRequestThreadReference != null && heightRequestThreadReference.IsAlive)
            {
                heightRequestThreadReference.Abort();
            }
        }
    }
}
