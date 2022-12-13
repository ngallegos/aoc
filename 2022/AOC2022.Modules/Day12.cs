using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC2022.Modules;

public class Day12 : DayBase
{
    public override bool Completed => true;
    private static List<(char letter, int height)> HeightMap = "abcdefghijklmnopqrstuvwxyz"
        .ToCharArray()
        .Select((c, i) => (c, i+1)).ToList();

    private List<string> _sampleInput;
    private List<string> _actualInput;

    public Day12()
    {
        _sampleInput = get_sample("Part1").ToList();
        _actualInput = get_input("Part1").ToList();
    }

    public override dynamic Part1()
    {
        throw new NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new NotImplementedException();
    }

    public override async Task<dynamic> Part1Async()
    {
        var _sampleMap = new ElevationMap(_sampleInput);
        var _actualMap = new ElevationMap(_actualInput);
        var _sampleRoute = await _sampleMap.GetBestPath();
        var _sampleVisualization = _sampleMap.GetVisualization();
        var _actualRoute = await _actualMap.GetBestPath();
        var _actualVisualization = _actualMap.GetVisualization();
        return new
        {
            SampleRoute = _sampleRoute.Count - 1,
            SampleVisualization = _sampleVisualization,
            ActualRoute = _actualRoute.Count - 1,
            ActualVisualization = _actualVisualization
        };
    }

    public override async Task<dynamic> Part2Async()
    {
        var _sampleMap = new ElevationMap(_sampleInput, 'a');
        var _actualMap = new ElevationMap(_actualInput, 'a');
        var _sampleRoute = await _sampleMap.GetBestPath();
        var _sampleVisualization = _sampleMap.GetVisualization();
        var _actualRoute = await _actualMap.GetBestPath();
        var _actualVisualization = _actualMap.GetVisualization();
        return new
        {
            SampleRoute = _sampleRoute.Count - 1,
            SampleVisualization = _sampleVisualization,
            ActualRoute = _actualRoute.Count - 1,
            ActualVisualization = _actualVisualization
        };
    }

    private class ElevationMap
    {
        private readonly int[][] _map;
        private List<RouteLocation> _startingLocations = new List<RouteLocation>();
        private RouteLocation _destination;
        private List<RouteLocation> _bestPath;
        
        private int[] this[int index] => _map[index];
        private int NorthBorder { get; set; }
        private int SouthBorder { get; set; }
        private int EastBorder { get; set; }
        private int WestBorder { get; set; }
        
        public ElevationMap(List<string> inputs, char additionalStartIdentifier = 'S')
        {
            var maxY = inputs.Count - 1;
            _map = inputs.Select((s, y) => {
                return s.ToCharArray().Select((c, x) =>
                {
                    if (c == 'S' || c == additionalStartIdentifier)
                    {
                        _startingLocations.Add(new RouteLocation(x, y, 1));
                        return 1;
                    }

                    if (c == 'E')
                    {
                        _destination = new RouteLocation(x, y, 26);
                        return 26;
                    }
                    return int.Parse(HeightMap.First(h => h.letter == c).height.ToString());
                }).ToArray(); }).ToArray();
            WestBorder = 0;
            SouthBorder = 0;
            NorthBorder = maxY;
            EastBorder = _map[0].Length - 1;
        }
        
        private List<RouteLocation> GetUnvisitedNeighbors(RouteLocation current, List<RouteLocation> _visited)
        {
            var cX = current.XCoordinate;
            var cY = current.YCoordinate;
            var neighbors = new List<RouteLocation>
            {
                new RouteLocation(cX + 1, cY, 0),
                new RouteLocation(cX - 1, cY, 0),
                new RouteLocation(cX, cY + 1, 0),
                new RouteLocation(cX, cY - 1, 0),
            };
            neighbors.RemoveAll(n => n.XCoordinate < WestBorder || n.XCoordinate > EastBorder
                                                                       || n.YCoordinate < SouthBorder || n.YCoordinate > NorthBorder);
            neighbors.ForEach(n =>
            {
                n.Elevation = this[n.YCoordinate][n.XCoordinate];
                n.Previous = current;
            });
            neighbors.RemoveAll(x => _visited.Any(d => d.ToString() == x.ToString()));
            neighbors.RemoveAll(ps => ps.Elevation > current.Elevation + 1);
            return neighbors.ToList();
        }

        public async Task<List<RouteLocation>> GetBestPath()
        {
            var possibleRoutes = new List<List<RouteLocation>>();
            var startChunks = _startingLocations.Chunk(10).ToList();
            foreach (var chunk in startChunks.Select((x, i) => new { startLocations = x, i }))
            {
                Console.WriteLine($"Processing chunk {chunk.i + 1} of {startChunks.Count}");
                var tasks = chunk.startLocations.Select(_start => Task.Run(() =>
                {
                    var q = new Queue<RouteLocation>();
                    RouteLocation current = null;
                    var _visited = new List<RouteLocation>();
                    _visited.Clear();
                    _visited.Add(_start);
                    q.Enqueue(_start);
                    while (q.Count > 0)
                    {
                        current = q.Dequeue();
                        if (current.ToString() == _destination.ToString())
                            break;

                        var potentialSteps = GetUnvisitedNeighbors(current, _visited);
                        foreach (var potentialStep in potentialSteps)
                        {
                            _visited.Add(potentialStep);
                            q.Enqueue(potentialStep);
                        }
                    }

                    return current.GetFullRoute();
                })).ToList();
                possibleRoutes.AddRange(await Task.WhenAll(tasks));
                possibleRoutes.RemoveAll(x => x.Last().ToString() != _destination.ToString());
                Console.WriteLine($"Found {possibleRoutes.Count} possible routes so far...");
            }

            _bestPath = possibleRoutes.MinBy(x => x.Count);
            return _bestPath;
        }

        public List<string> GetVisualization()
        {
            var visualization = new List<string>();
            for (int y = 0; y < _map.Length; y++)
            {
                visualization.Add(new string(Enumerable.Range(0, _map[0].Length).Select(x => '.').ToArray()));
            }

            foreach (var location in _bestPath)
            {
                if (location.Previous == null)
                    continue;
                var previous = location.Previous;
                var direction = '>';
                if (previous.XCoordinate > location.XCoordinate)
                    direction = '<';
                else if (previous.YCoordinate < location.YCoordinate)
                    direction = 'v';
                else if (previous.YCoordinate > location.YCoordinate)
                    direction = '^';
                var row = visualization[previous.YCoordinate];
                row = row.Remove(previous.XCoordinate, 1).Insert(previous.XCoordinate, direction.ToString());
                visualization[previous.YCoordinate] = row;
            }

            return visualization;
        }
    }

    private class RouteLocation
    {
        public RouteLocation(int x, int y, int elevation)
        {
            XCoordinate = x;
            YCoordinate = y;
            Elevation = elevation;
        }
        public int XCoordinate { get; private set; }
        public int YCoordinate { get; private set; }
        public int Elevation { get; set; }

        public override string ToString()
        {
            return $"({XCoordinate},{YCoordinate})";
        }
        
        public RouteLocation Previous { get; set; }

        public List<RouteLocation> GetFullRoute()
        {
            var route = new List<RouteLocation>();
            var current = this;
            while (current != null)
            {
                route.Add(current);
                current = current.Previous;
            }

            route.Reverse();
            return route;
        }
    }
}