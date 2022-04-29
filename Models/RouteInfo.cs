namespace BestBusRoute.Models
{
    public static class RouteInfo
    {
        public static int NumberOfBuses { get; set; }
        public static IList<Bus> Buses { get; set; }
        public static int TotalNumOfStops { get; set; }
        public static IList<Stop> Stops { get; set; }

        static RouteInfo()
        {
            Buses = new List<Bus>();
            Stops = new List<Stop>();

        }

        public static void Clear()
        {
            NumberOfBuses = 0;
            Buses.Clear();
            TotalNumOfStops = 0;
            Stops.Clear();
        }
    }
}
