using System.Web.Mvc;
using AspNetSample.Models;
using SampleShared;

namespace AspNetSample.Controllers
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