using System.Web.Http;
using GSS.DependencyInjection.Testing.Fakes;

namespace GSS.DependencyInjection.WebApi.Tests
{
    public class FakeApiController : ApiController
    {
        public FakeApiController(IFakeService fakeService)
        {
            FakeService = fakeService;
        }

        public IFakeService FakeService { get; }
    }
}
