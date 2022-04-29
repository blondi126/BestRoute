using BestBusRoute.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestBusRoute.Controllers
{
    [ApiController]
    [Route("/api/stops/")]
    public class StopsController: Controller
    {
        [HttpGet]
        public IActionResult GetStops()
        {
            var stops = new int[RouteInfo.TotalNumOfStops];
            for (var i = 1; i <= RouteInfo.TotalNumOfStops; i++)
            {
                stops[i - 1] = i;
            }

            return Ok(stops);
        }
    }
}
