using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AirHelp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Login",
               url: "login/{action}/{id}",
               defaults: new { controller = "login", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Edit",
              url: "login/edit",
              defaults: new { controller = "login", action = "Edit", id = UrlParameter.Optional }
          );

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
