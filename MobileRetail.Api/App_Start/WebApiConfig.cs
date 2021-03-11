using System.Web.Http;
using System.Web.Http.Cors;

namespace MobileRetail.Api
{
    /// <summary>
    /// Web API Configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static HttpConfiguration Register(HttpConfiguration config)
        {
            if (config != null)
            {
                // Enable Cors
                var cors = new EnableCorsAttribute("*", "*", "*");
                config.EnableCors(cors);

                // Web API attribute routes
                config.MapHttpAttributeRoutes();

                // Web API dafault routes
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional });

                // Remove the XML formatter
                config.Formatters.Remove(config.Formatters.XmlFormatter);
            }

            return config;
        }
    }
}
