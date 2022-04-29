namespace BestBusRoute.Algorithms
{
    public class CheapModelWithDfs
    {
        //Храним пройденные вершины
        private static readonly Stack<int> VisitedStops = new();
        private static readonly Stack<int> UsedBuses = new();

        private static List<int> _bestRouteStops = new();
        private static List<int> _bestRouteBuses = new();

        private static List<List<Tuple<int, int, int>>> _graph = null!;

        //Остановка назначения
        private static int _dist;

        //Искомая стоимость
        private static int _result;

        public static CheapModelResult Calculate(List<List<Tuple<int, int, int>>> graph, int sourceStop,
            int destinationStop)
        {
            _graph = graph;
            _dist = destinationStop;
            _result = int.MaxValue;

            Dfs(sourceStop - 1, 0, 0);
            var result = new CheapModelResult(_bestRouteStops, _bestRouteBuses, _result);

            Print();
            VisitedStops.Clear();
            UsedBuses.Clear();
            return result;
        }

        //Рекурсивно обходим дерево в глубину.
        //Если посещаем вершину, через которую на текущем маршруте проходили, возвращаемся назад
        //Если попадаем в искомую вершину, то смотрим на текущую стоимость:
        //Меньше - сохраняем её и маршрут
        //Больше - ищем дальше 
        private static void Dfs(int ver, int currentCost, int currentBus)
        {
            VisitedStops.Push(ver+1);
            UsedBuses.Push(currentBus);

            if (ver == _dist - 1)
            {
                if (currentCost < _result)
                {
                    _result = currentCost;
                    _bestRouteStops = VisitedStops.Reverse().ToList();
                    _bestRouteBuses = UsedBuses.Reverse().ToList();
                }
                return;
            }

            for (var i = 0; i < _graph[ver].Count; i++)
            {
                var to = _graph[ver][i];

                if (VisitedStops.Contains(to.Item1))
                    continue;

                var cost = currentBus == to.Item3 ? currentCost : currentCost + to.Item2;

                Dfs(to.Item1 - 1, cost, to.Item3);
            }

            VisitedStops.Pop();
            UsedBuses.Pop();
        }

        private static void Print()
        {
            Console.WriteLine($"Минимальная стоимость дороги до остановки №{_dist}: {_result}.");
            Console.WriteLine("Остановки:");
            foreach (var bestRouteStop in _bestRouteStops)
            {
                Console.Write(bestRouteStop + " ");
            }

            Console.WriteLine("\nСоответствующие для каждой остановки автобусы:");
            foreach (var bestRouteBuses in _bestRouteBuses)
            {
                if(bestRouteBuses != 0)
                 Console.Write(bestRouteBuses + " ");
            }

            Console.WriteLine("\n\n");
        }
    }
}
