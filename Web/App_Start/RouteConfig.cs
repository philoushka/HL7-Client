using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HL7_TCP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", new { controller = "HL7", action = "Index" });
            routes.MapRoute("SendHL7Message", "SendHL7Message", new { controller = "HL7", action = "SendHL7Message" });
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "HL7", action = "Index"  }
            );
        }
    }
}