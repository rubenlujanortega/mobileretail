using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using System.Data.SqlClient;
using System.Configuration;
using ClientProducts.Application;
using ClientProducts.Repository;
using ClientProducts.Logs;
using SqlClient.Helper;

namespace MobileRetail.Api
{
    public static class UnityConfig
    {
        /// <summary>
        /// Registers the components
        /// </summary>
        /// <param name="container">The container.</param>
        private static IUnityContainer RegisterComponents(this IUnityContainer container)
        {
            if (container != null)
            {
                //Register database
                container.RegisterDatabase("RAWRAPSIIF", "RAWRAPSIIF");
                container.RegisterDatabase("RA", "RA");
                container.RegisterDatabase("SecurityGlobalApp", "SecurityGlobalApp");
                container.RegisterDatabase("DOMICILIACION", "DOMICILIACION");

                //Register BL
                container.RegisterType<IClientProductsApp, ClientProductsApp>();

                //Register DA
                container.RegisterType<IRAWRAPSIIFDa, RAWRAPSIIFDa>();
                container.RegisterType<IRADa, RADa>();
                container.RegisterType<ISecurityGlobalAppDa, SecurityGlobalAppDa>();
                container.RegisterType<IDOMICILIACIONDa, DOMICILIACIONDa>();

                //Register Proxy
                container.RegisterType<INotificationsProxy, NotificationsProxy>();

                //Register Helpers
                container.RegisterType<ISeriLogHelper, SeriLogHelper>();
                container.RegisterFactory<SeriLogHelper>("SeriLog", c => new SeriLogHelper(), new HierarchicalLifetimeManager());

                container.RegisterType<ISqlClientHelper, SqlClientHelper>();
            }
            return container;
        }

        /// <summary>
        /// Registers the components.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static HttpConfiguration Register(HttpConfiguration config)
        {
            if (config != null)
            {
                var container = new UnityContainer().RegisterComponents();
                config.DependencyResolver = new UnityDependencyResolver(container);
            }

            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IUnityContainer RegisterDatabase(this IUnityContainer container, string name, string connection)
        {
            if (container != null)
            {
                container.RegisterFactory<SqlConnection>(name, c => new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ConnectionString), new HierarchicalLifetimeManager());
            }

            return container;

        }
    }
}