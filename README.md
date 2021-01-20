# GSS.DependencyInjection.AspNet

[![Build Status][ci-badge]][ci] [![Code Coverage][codecov-badge]][codecov]

[ci]: https://github.com/akunzai/GSS.DependencyInjection.AspNet/actions?query=workflow%3ACI
[ci-badge]: https://github.com/akunzai/GSS.DependencyInjection.AspNet/workflows/CI/badge.svg
[codecov]: https://codecov.io/gh/akunzai/GSS.DependencyInjection.AspNet
[codecov-badge]: https://codecov.io/gh/akunzai/GSS.DependencyInjection.AspNet/branch/main/graph/badge.svg?token=EO9CL9XXRU

Integrate ASP.NET MVC/WebApi with [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)

## NuGet Packages

- [GSS.DependencyInjection.Mvc ![NuGet version](https://img.shields.io/nuget/v/GSS.DependencyInjection.Mvc.svg?style=flat-square)](https://www.nuget.org/packages/GSS.DependencyInjection.Mvc/)
- [GSS.DependencyInjection.WebApi ![NuGet version](https://img.shields.io/nuget/v/GSS.DependencyInjection.WebApi.svg?style=flat-square)](https://www.nuget.org/packages/GSS.DependencyInjection.WebApi/)

## Getting Started

### ASP.NET MVC

ASP.NET MVC requires the [GSS.DependencyInjection.Mvc](https://www.nuget.org/packages/GSS.DependencyInjection.Mvc/)

```csharp
using System.Web.Mvc;
using GSS.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
...
// Configure your services
var services = new ServiceCollection();
...

// Register your MVC controllers
services.AddMvcControllers(Assembly.GetExecutingAssembly());

// Build the service resolver
var resolver = collection.BuildServiceProvider();

// Replace the dependency resolver
DependencyResolver.SetResolver(new MicrosoftMvcDependencyResolver(resolver));
...
```

### ASP.NET Web API

ASP.NET Web API requires the [GSS.DependencyInjection.WebApi](https://www.nuget.org/packages/GSS.DependencyInjection.WebApi/)

#### Hosting on IIS with `Global.asax`

> Web API must be configured before MVC

```csharp
using System.Web.Http;
using GSS.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
...
    protected void Application_Start()
    {
      // Configure your services
      var services = new ServiceCollection();
      ...

      // Register your API controllers
      services.AddApiControllers(Assembly.GetExecutingAssembly());

      // Build the service resolver
      var resolver = collection.BuildServiceProvider();

      var webApiConfig = GlobalConfiguration.Configuration;

      // Replace the dependency resolver
      webApiConfig.DependencyResolver = new MicrosoftWebApiDependencyResolver(resolver);
      ...
      webApiConfig.EnsureInitialized();
    }
...
```

#### Hosting on OWIN with `Startup`

```csharp
using System.Web.Http;
using GSS.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
...
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure your services
            var services = new ServiceCollection();
            ...

            // Register your API controllers
            services.AddApiControllers(Assembly.GetExecutingAssembly());

            // Build the service resolver
            var resolver = collection.BuildServiceProvider();

            var webApiConfig = new HttpConfiguration();

            // Replace the dependency resolver
            webApiConfig.DependencyResolver = new MicrosoftWebApiDependencyResolver(resolver);
            ...
            webApiConfig.EnsureInitialized();

            app.UseWebApi(webApiConfig);
        }
    }
...
```
