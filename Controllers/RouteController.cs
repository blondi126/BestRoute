using BestBusRoute.Models;
using BestBusRoute.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BestBusRoute.Controllers
{
    [ApiController]
    [Route("/api/route/")]
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public IActionResult GetRoute(int src, int dst, string type, int hrs, int min)
        {
            switch (type)
            {
                case "fastest":
                {
                    var result = _routeService.GetFastestRoute(src, dst, (hrs, min));
                    return Ok(new {buses = result.Buses, stops = result.Stops, totalTime = result.Time});
                }
                case "cheapest":
                {
                    var result = _routeService.GetCheapestRoute(src, dst);
                    return Ok(new { buses = result.Buses, stops = result.Stops, totalCost = result.Cost });
                }
                default:
                    return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            RouteInfo.Clear();
            _routeService.ParseBusRouteInfo(file);
            return Ok();
        }
    }
}
