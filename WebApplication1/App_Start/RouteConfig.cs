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

            routes.MapRoute("displayLines", "display/{ip}/{port}/{time}",
                defaults: new { controller = "Fs", action = "displayLines" }
            );
            
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}",
               defaults: new { controller = "Fs", action = "Index"}
            );
            
        }
    }
}
