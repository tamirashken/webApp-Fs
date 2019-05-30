﻿using System;
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

            /*
            routes.MapRoute(
                name: "Default",
                url: "display/{ip}/{port}",
                defaults: new { controller = "Fs", action = "display", ip = "127.0.0.1", port = "5400" }
            );
            */

            routes.MapRoute("display", "display/{ip}/{port}",
                defaults: new { controller = "Fs", action = "display" }
            );
        }
    }
}
