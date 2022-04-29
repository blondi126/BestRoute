namespace BestBusRoute.Algorithms
{
    public class CheapModelResult
    {
        public int[] Stops;
        public int[] Buses;
        public int Cost { get; set; }

        public CheapModelResult(IEnumerable<int> stops, IEnumerable<int> buses, int cost)
        {
            Stops = stops.Reverse().ToArray();
            Buses = buses.Reverse().ToArray();
            Cost = cost;
        }
    }
}
