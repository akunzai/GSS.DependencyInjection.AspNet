using System.Linq;
using System.Reflection;
using GSS.DependencyInjection.Mvc.Tests;
using GSS.DependencyInjection.Testing.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// ReSharper disable once CheckNamespace
namespace GSS.DependencyInjection.MVC.Tests
{
    public class MvcDependencyResolverTest
    {
        [Fact]
        public void SingleServiceCanBeResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            var resolver = services.BuildServiceProvider();
            var mvcResolver = new MicrosoftMvcDependencyResolver(resolver);
            
            // Act
            var actual = mvcResolver.GetService(typeof(IFakeService));

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
            var mvcResolver = new MicrosoftMvcDependencyResolver(resolver);

            // Act
            var actual = mvcResolver.GetService(typeof(IFakeService));

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
            var mvcResolver = new MicrosoftMvcDependencyResolver(resolver);

            // Act
            var actual = mvcResolver.GetServices(typeof(IFakeService));

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
            var mvcResolver = new MicrosoftMvcDependencyResolver(resolver);

            // Act
            var actual = mvcResolver.GetServices(typeof(IFakeService));

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ControllerWithServiceCanBeResolved()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IFakeService, FakeService>();
            services.AddMvcControllers(Assembly.GetExecutingAssembly());
            var resolver = services.BuildServiceProvider();
            var mvcResolver = new MicrosoftMvcDependencyResolver(resolver);

            // Act
            var actual = mvcResolver.GetService(typeof(FakeController)) as FakeController;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<FakeService>(actual.FakeService);
        }
    }
}
