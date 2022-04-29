using BestBusRoute.Algorithms;
using BestBusRoute.Models;
using BestBusRoute.Services.Interfaces;

namespace BestBusRoute.Services
{
    public class RouteService : IRouteService
    {
        public void ParseBusRouteInfo(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            //Заполняем количество автобусов и остановок
            RouteInfo.NumberOfBuses = int.Parse(reader.ReadLine()!);
            RouteInfo.TotalNumOfStops = int.Parse(reader.ReadLine()!);

            for (var i = 0; i < RouteInfo.TotalNumOfStops;)
            {
                RouteInfo.Stops.Add(new Stop(++i));
            }

            //Заполняем время отправления автобусов
            var timesString = reader.ReadLine();
            var times = timesString!.Split(' ');
            for (byte i = 0; i < RouteInfo.NumberOfBuses;)
            {
                var time = times[i].Split(':');
                RouteInfo.Buses.Add(new Bus(++i, new TimeOnly(int.Parse(time[0]), int.Parse(time[1]))));
            }

            //Заполняем цену проезда автобусов 
            var faresString = reader.ReadLine();
            var fares = faresString!.Split(' ');
            for (var i = 0; i < RouteInfo.NumberOfBuses; i++)
            {
                RouteInfo.Buses[i].Fare = int.Parse(fares[i]);
            }

            //Заполняем информацию о маршрутах автобуса
            for (var i = 0; i < RouteInfo.NumberOfBuses; i++)
            {
                var routeInfo = reader.ReadLine()!.Split(' ');
                var numStops = int.Parse(routeInfo[0]);
                RouteInfo.Buses[i].CountOfStops = numStops;

                var routeTime = 0;
                for (var j = 1; j <= numStops; j++)
                {
                    routeTime += int.Parse(routeInfo[j + numStops]);
                    RouteInfo.Buses[i].ListStops.Add(int.Parse(routeInfo[j]));

                    RouteInfo.Buses[i].StopIdTimeToNextStopPairs.Add(
                        int.Parse(routeInfo[j]),
                        int.Parse(routeInfo[j + numStops])
                    );
                }
                RouteInfo.Buses[i].RouteTimeInMinutes = routeTime;
            }
        }

        public FastModelResult GetFastestRoute(int source, int destination, (int,int) time)
        {
            //Представление данных в виде  ориентированного взвешенного графа, где
            //первое число - соседняя вершина (номер следующей в маршруте остановки)
            //второе число - вес (время дороги между остановками)
            //третье число - id автобуса
            List<List<Tuple<int, int, int>>> tree = new();

            for (var i = 0; i < RouteInfo.TotalNumOfStops; i++)
            {
                tree.Add(new List<Tuple<int, int, int>>());
            }

            //Инициализация графа
            foreach (var bus in RouteInfo.Buses)
            {
                for (var i = 1; i < bus.ListStops.Count; i++)
                {
                    tree[bus.ListStops[i - 1] - 1].Add(Tuple.Create(bus.ListStops[i], bus.StopIdTimeToNextStopPairs[bus.ListStops[i - 1]], bus.Id));
                }
                tree[bus.ListStops.Last() - 1].Add(Tuple.Create(bus.ListStops[0], bus.StopIdTimeToNextStopPairs[bus.ListStops.Last()], bus.Id));
            }

            //Вычисление быстрейшего маршрута
            var result = FastModelAlgorithm.Calculate(tree, source, destination, time);
            return result;
        }

        public CheapModelResult GetCheapestRoute(int source, int destination)
        {
            //Представление данных в виде  ориентированного взвешенного графа, где
            //первое число - соседняя вершина (номер следующей в маршруте остановки)
            //второе число - вес (цена поездки)
            //третье число - id автобуса
            List<List<Tuple<int, int, int>>> tree = new();

            for (var i = 0; i < RouteInfo.TotalNumOfStops; i++)
            {
                tree.Add(new List<Tuple<int, int, int>>());
            }

            //Инициализация графа
            foreach (var bus in RouteInfo.Buses)
            {
                for (var i = 1; i < bus.ListStops.Count; i++)
                {
                    tree[bus.ListStops[i-1]-1].Add(Tuple.Create(bus.ListStops[i],bus.Fare,bus.Id));
                }
                tree[bus.ListStops.Last()-1].Add(Tuple.Create(bus.ListStops[0], bus.Fare,bus.Id));
            }

            //Вычисление наидешёвого маршрута
            return CheapModelWithDfs.Calculate(tree, 4, 1);
        }

        public void PrintInfo()
        {
            Console.WriteLine("Список автобусов:");
            foreach (var bus in RouteInfo.Buses)
            {
                Console.WriteLine($"Автобус №{bus.Id}.");
                Console.WriteLine($"Цена: {bus.Fare}");
                Console.WriteLine($"Время движения: {bus.StartTime}-{bus.EndTime}");
                Console.WriteLine($"Маршрут: ");
                foreach (var (key, value) in bus.StopIdTimeToNextStopPairs)
                {
                    Console.WriteLine($"Остановка {key} - время в пути до следующей {value}");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("Список остановок:");
            foreach (var stop in RouteInfo.Stops)
            {
                Console.WriteLine($"Остановка №{stop.Id}");
                var busId = 1;
                foreach (var bus in stop.BusArrivalTimes)
                {
                    Console.Write($"Автобус {busId++}:");
                    foreach (var time in bus)
                    {
                        Console.Write($"{time} ");
                    }
                    Console.WriteLine("\n");
                }

                Console.WriteLine("\n");
            }
        }
    }
}