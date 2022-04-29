namespace BestBusRoute.Algorithms
{
    public class FastModelResult
    {
        public int[] Stops;
        public int[] Buses;
        public int Time { get; set; }

        public FastModelResult(Stack<int> stops, Stack<int> buses)
        {
            Stops = stops.ToArray();
            Buses = buses.ToArray();
        }
    }
}
