using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Mvc;

namespace GSS.DependencyInjection
{
    /// <summary>
    /// implementation of the <see cref="IDependencyResolver"/>
    /// </summary>
    /// <inheritdoc />
    public class MicrosoftMvcDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _services;

        public MicrosoftMvcDependencyResolver(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public object GetService(Type serviceType)
        {
            return _services.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _services.GetServices(serviceType);
        }
    }
}