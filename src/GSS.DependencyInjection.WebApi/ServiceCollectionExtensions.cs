using System;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace GSS.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding controllers to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Web API controllers in <paramref name="controllerAssembly" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="controllerAssembly">The assembly of the controllers to register</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddApiControllers(
            this IServiceCollection services,
            Assembly controllerAssembly)
        {
            if (controllerAssembly == null)
                throw new ArgumentNullException(nameof(controllerAssembly));
            var controllerTypes = controllerAssembly.GetExportedTypes()
                .Where(t => !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && typeof(IHttpController).IsAssignableFrom(t)
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }
            return services;
        }
    }
}
