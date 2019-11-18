using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace GSS.DependencyInjection
{
    /// <summary>
    /// implementation of the <see cref="IDependencyResolver"/>
    /// </summary>
    /// <inheritdoc />
    public class MicrosoftWebApiDependencyResolver : IDependencyResolver
    {
        private bool _disposed;
        private readonly IServiceProvider _services;
        private readonly IServiceScope _scope;

        public MicrosoftWebApiDependencyResolver(IServiceProvider services) : this(services, false)
        {
        }

        internal MicrosoftWebApiDependencyResolver(IServiceProvider services, bool beginScope)
        {
            if (beginScope)
            {
                var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
                _scope = scopeFactory.CreateScope();
                _services = _scope.ServiceProvider;
            }
            else
            {
                _services = services;
            }
        }

        public object GetService(Type serviceType)
        {
            return _services.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _services.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new MicrosoftWebApiDependencyResolver(_services, true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _scope?.Dispose();
            }

            _disposed = true;
        }
    }
}