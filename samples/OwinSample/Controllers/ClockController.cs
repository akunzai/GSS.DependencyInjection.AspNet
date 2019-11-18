using System;
using System.Web.Http;
using SampleShared;

namespace OwinSample.Controllers
{
    public class ClockController : ApiController
    {
        private readonly ISystemClock _clock;

        public ClockController(ISystemClock clock)
        {
            _clock = clock;
        }

        // GET /api/clock
        public DateTime Get()
        {
            return _clock.Now;
        }
    }
}
