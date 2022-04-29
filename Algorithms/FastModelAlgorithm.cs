using BestBusRoute.Models;

namespace BestBusRoute.Algorithms
{
    public class FastModelAlgorithm
    {
        public static FastModelResult Calculate(List<List<Tuple<int, int, int>>> graph, int sourceStop, int destinationStop,
            (int, int) time)
        {
            //Минимальные расстояния до вершин (остановок) от первоначальной
            var minTime = new int[RouteInfo.TotalNumOfStops];

            //Сохраняем ожидания автобуса для вывода общего пути
            var minAdditionalWaiting = new int[RouteInfo.TotalNumOfStops];

            //Посещённые вершины
            var isVisited = new bool[RouteInfo.TotalNumOfStops];

            for (var i = 0; i < RouteInfo.TotalNumOfStops; i++)
            {
                minTime[i] = int.MaxValue;
                isVisited[i] = false;
            }

            minTime[sourceStop - 1] = 0;

            for (var count = 0; count < RouteInfo.TotalNumOfStops - 1; count++)
            {
                //Функция возвращает непосещённую вершину с наименьшим расстоянием
                //для обхода её соседей
                var u = GetStopWithMinimumTime(minTime, isVisited, RouteInfo.TotalNumOfStops);

                if (minTime[u] == int.MaxValue)
                    break;

                isVisited[u] = true;

                //Для каждой вершины, в которую можно попасть из текущей, высчитываем минимальное расстояние
                foreach (var (stopId, len, busId) in graph[u])
                {
                    var (hours, minutes) = time;
                    var totalMinutes = hours * 60 + minutes + minTime[u];

                    //Вычисляем следующее время прибытия автобуса на текущей остановке
                    var nextBusTime = GetNextBusTime(u + 1, busId, (totalMinutes / 60 % 24, totalMinutes % 60));

                    var waitingTime = (nextBusTime.Item1 * 60 + nextBusTime.Item2) -
                                      (hours * 60 + minutes + minTime[u]);

                    if (len + waitingTime + minTime[u] < minTime[stopId - 1])
                    {
                        minTime[stopId - 1] = len + waitingTime + minTime[u];
                        minAdditionalWaiting[u] = waitingTime;
                    }

                }
            }

            //Вывод в консоль
            Print(minTime, RouteInfo.TotalNumOfStops);

            //Восстановление пути от изначальной до требуемой остановки
            var result = GetPath(graph, minTime, minAdditionalWaiting, sourceStop - 1, destinationStop - 1);
            result.Time = minTime[destinationStop - 1];

            PrintPath(result, sourceStop, destinationStop);

            return result;
        }


        private static int GetStopWithMinimumTime(IReadOnlyList<int> minTime, IReadOnlyList<bool> isVisited, int stopsCount)
        {
            var min = int.MaxValue;
            var minIndex = 0;

            for (var i = 0; i < stopsCount; ++i)
            {
                if (isVisited[i] || minTime[i] > min)
                    continue;

                min = minTime[i];
                minIndex = i;
            }

            return minIndex;
        }

        private static (int, int) GetNextBusTime(int stopId, int busId, (int, int) currentTime)
        {
            var firstTimeArrived = RouteInfo.Buses[busId - 1].StartTime;
            var currentStopId = RouteInfo.Buses[busId - 1].ListStops[0];

            //Вычисляем первое прибытие автобуса на нужную нам остановку
            var counter = 0;
            while (currentStopId != stopId)
            {
                firstTimeArrived =
                    firstTimeArrived.AddMinutes(RouteInfo.Buses[busId - 1].StopIdTimeToNextStopPairs[currentStopId]);
                counter++;
                currentStopId = RouteInfo.Buses[busId - 1].ListStops[counter];
            }

            //Вычисляем время прибытия ближайшего автобуса
            var nextTimeArrived = firstTimeArrived;
            var curTime = new TimeOnly(currentTime.Item1, currentTime.Item2);
            while (nextTimeArrived < curTime)
            {
                nextTimeArrived = nextTimeArrived.AddMinutes(RouteInfo.Buses[busId - 1].RouteTimeInMinutes);
            }

            return (nextTimeArrived.Hour, nextTimeArrived.Minute);
        }

        private static void Print(IReadOnlyList<int> minDistance, int stopsCount)
        {
            Console.WriteLine("Вершина    Время дороги от заданной остановки");

            for (var i = 0; i < stopsCount; ++i)
                Console.WriteLine("{0}\t  {1}", i + 1, minDistance[i]);
        }

        private static FastModelResult GetPath(List<List<Tuple<int, int, int>>> tree, IReadOnlyList<int> minTime, IReadOnlyList<int> minAddTime, int source, int destination)
        {
            var path = new Stack<int>();
            var buses = new Stack<int>();

            //Заносим искомую вершину в ответ и записываем её вес для поиска предыдущей
            var end = destination; 
            path.Push(end + 1); 
            var weight = minTime[end];

            //Выполняем пока не дойдём до первоначальной вершины
            while (end != source) 
            {
                for (var i = 0; i < RouteInfo.TotalNumOfStops; i++) 
                {
                    foreach (var (stopId, len, busId) in tree[i])
                    {
                        if (stopId - 1 == end)
                        {
                            var temp = weight - len; 

                            //Если рассчитанный вес равен минимальному расстоянию до вершины
                            //+ ожиданию, то это искомая вершина
                            if (temp == minTime[i] + minAddTime[i]) 
                            {
                                weight = temp; 
                                end = i; 
                                path.Push(i + 1); 
                                buses.Push(busId);
                                break;
                            }
                        }
                    }
                }
            }
            return new FastModelResult(path, buses);
        }

        private static void PrintPath(FastModelResult result, int src, int dst)
        {
            Console.WriteLine($"Минимальное время дороги до остановки №{dst}: {result.Time}.");
            Console.WriteLine("Остановки:");
            foreach (var resultStop in result.Stops)
                Console.Write(resultStop + " ");
            

            Console.WriteLine("\nСоответствующие для каждой остановки автобусы:");
            foreach (var resultBus in result.Buses)
                Console.Write(resultBus + " ");
            Console.WriteLine("\n\n");
        }

    }
}