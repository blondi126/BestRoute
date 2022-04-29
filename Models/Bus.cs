namespace BestBusRoute.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; } 
        public int Fare { get; set; }
        public int CountOfStops { get; set; }
        public int RouteTimeInMinutes { get; set; }
        public List<int> ListStops { get; set; }
        public Dictionary<int,int> StopIdTimeToNextStopPairs { get; set; }

        public Bus(byte id, TimeOnly startTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = new TimeOnly(23, 59, 59, 59);
            StopIdTimeToNextStopPairs = new Dictionary<int,int>();
            ListStops = new List<int>();
        }
    }
}
