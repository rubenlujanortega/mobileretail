using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(MobileRetail.Api.Startup))]

namespace MobileRetail.Api
{
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">OWIN App builder</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            UnityConfig.Register(config);
            SwaggerConfig.Register(config);

            app.UseWebApi(config);
        }
    }
}
