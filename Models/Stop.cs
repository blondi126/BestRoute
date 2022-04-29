namespace BestBusRoute.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public List<List<TimeOnly>> BusArrivalTimes { get; set; }

        public Stop(int id)
        {
            Id = id;
            BusArrivalTimes = new List<List<TimeOnly>>(RouteInfo.NumberOfBuses);
        }

    }
}
