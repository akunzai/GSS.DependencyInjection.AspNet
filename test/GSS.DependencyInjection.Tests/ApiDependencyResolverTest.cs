using System.Linq;
using System.Reflection;
using GSS.DependencyInjection.Testing.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// ReSharper disable once CheckNamespace
namespace GSS.DependencyInjection.WebApi.Tests
{
    public class ApiDependencyResolverTest
    {
        [Fact]
        public void SingleServiceCanBeResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var actual = apiResolver.GetService(typeof(IFakeService));

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<FakeService>(actual);
        }

        [Fact]
        public void SingleServiceCanBeOptionalResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var actual = apiResolver.GetService(typeof(IFakeService));

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void MultipleServiceCanBeResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var actual = apiResolver.GetServices(typeof(IFakeService));

            // Assert
            Assert.NotNull(actual);
            var collection = actual as object[] ?? actual.ToArray();
            Assert.Single(collection);
            Assert.IsType<FakeService>(collection.Single());
        }

        [Fact]
        public void MultipleServiceCanBeOptionalResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var actual = apiResolver.GetServices(typeof(IFakeService));

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ControllerWithServiceCanBeResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            services.AddApiControllers(Assembly.GetExecutingAssembly());
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var actual = apiResolver.GetService(typeof(FakeApiController)) as FakeApiController;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<FakeService>(actual.FakeService);
        }

        [Fact]
        public void DisposableServiceCanBeDisposed()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            var resolver = services.BuildServiceProvider();
            using var apiResolver = new MicrosoftWebApiDependencyResolver(resolver);

            // Act
            var scope = apiResolver.BeginScope();
            var actual = scope.GetService(typeof(IFakeService)) as FakeService;
            scope.Dispose();

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Disposed);
        }
    }
}
