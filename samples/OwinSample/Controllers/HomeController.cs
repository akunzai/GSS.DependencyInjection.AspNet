using System.Web.Mvc;
using OwinSample.Models;
using SampleShared;

namespace OwinSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISystemClock _clock;

        public HomeController(ISystemClock clock)
        {
            _clock = clock;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ClockModel { Now = _clock.Now });
        }
    }
}