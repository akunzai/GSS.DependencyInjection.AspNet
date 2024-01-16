using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GSS.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleShared;

namespace AspNetSample
{
    public class MvcApplication : HttpApplication
    {
        private static IConfiguration _configuration;
        private static IServiceProvider _resolver;

        protected void Application_Start()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _resolver = services.BuildServiceProvider();

            // Web API must be configured before MVC
            var webApiConfig = GlobalConfiguration.Configuration;
            webApiConfig.MapHttpAttributeRoutes();
            webApiConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            webApiConfig.DependencyResolver = new MicrosoftWebApiDependencyResolver(_resolver);
            webApiConfig.EnsureInitialized();

            // MVC
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            DependencyResolver.SetResolver(new MicrosoftMvcDependencyResolver(_resolver));
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMvcControllers(assembly);
            services.AddApiControllers(assembly);
            var systemClock = _configuration.GetValue<DateTime?>("SystemClock");
            if (systemClock.HasValue)
            {
                services.AddSingleton<ISystemClock>(new DummySystemClock(systemClock.Value));
            }
            else
            {
                services.AddSingleton<ISystemClock, DefaultSystemClock>();
            }
        }
    }
}
