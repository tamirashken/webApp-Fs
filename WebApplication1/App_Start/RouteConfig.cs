using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("display", "display/{ip}/{port}",
                defaults: new { controller = "Fs", action = "display" }
            );

            routes.MapRoute("displayLines", "display/{ip}/{port}/{freq}",
                defaults: new { controller = "Fs", action = "displayLines" }
            );

            routes.MapRoute("saveFlightDetails", "save/{ip}/{port}/{freq}/{totalSec}/{fileName}",
                defaults: new { controller = "Fs", action = "saveFlightDetails" }
            );

            routes.MapRoute(
               name: "Default",
                url: "display/{action}/{id}",
               defaults: new { controller = "Fs", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
