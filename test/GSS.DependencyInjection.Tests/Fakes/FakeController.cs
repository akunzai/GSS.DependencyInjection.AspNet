using System.Web.Mvc;
using GSS.DependencyInjection.Testing.Fakes;

namespace GSS.DependencyInjection.Mvc.Tests
{
    public class FakeController : Controller
    {
        public FakeController(IFakeService fakeService)
        {
            FakeService = fakeService;
        }

        public IFakeService FakeService { get; }
    }
}
