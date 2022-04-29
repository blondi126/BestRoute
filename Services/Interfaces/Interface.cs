using BestBusRoute.Algorithms;

namespace BestBusRoute.Services.Interfaces
{
    public interface IRouteService
    {
        void ParseBusRouteInfo(IFormFile file);
        FastModelResult GetFastestRoute(int source, int destination, (int, int) time);
        CheapModelResult GetCheapestRoute(int source, int destination);
        void PrintInfo();
    }
}
