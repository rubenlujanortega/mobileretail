using Swashbuckle.Application;
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace MobileRetail.Api
{
    /// <summary>
    /// Swagger configuratio
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static HttpConfiguration Register(HttpConfiguration config)
        {
            if (config != null)
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\bin\";
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);

                config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "MobileRetailService API");
                    c.IncludeXmlComments(commentsFile);
                    c.RootUrl(req => new Uri(req.RequestUri, HttpContext.Current.Request.ApplicationPath ?? string.Empty).ToString());
                }).EnableSwaggerUi();
            }

            return config;
        }
    }
}