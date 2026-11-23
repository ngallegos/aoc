using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day12 : DayBase
{
    public override bool Ignore => true;
    private static List<(char letter, int height)> HeightMap = "abcdefghijklmnopqrstuvwxyz"
        .ToCharArray()
        .Select((c, i) => (c, i+1)).ToList();

    public override dynamic Part1()
    {
        var _sampleMap = new ElevationMap(get_sample().ToList());
        var _actualMap = new ElevationMap(get_input().ToList());
        var _sampleRoute = _sampleMap.GetBestPathFromStartLocation();
        var _sampleVisualization = _sampleMap.GetVisualization();
        var _actualRoute = _actualMap.GetBestPathFromStartLocation();
        var _actualVisualization = _actualMap.GetVisualization();
        return new
        {
            SampleRoute = _sampleRoute.Count - 1,
            SampleVisualization = _sampleVisualization,
            ActualRoute = _actualRoute.Count - 1,
            ActualVisualization = _actualVisualization
        };
    }

    public override dynamic Part2()
    {
        var _sampleMap = new ElevationMap(get_sample().ToList());
        var _actualMap = new ElevationMap(get_input().ToList());
        var _sampleRoute = _sampleMap.GetBestOverallPath();
        var _sampleVisualization = _sampleMap.GetVisualization();
        var _actualRoute = _actualMap.GetBestOverallPath();
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
        private RouteLocation _start;
        private RouteLocation _destination;
        private List<RouteLocation> _bestPath;
        
        private int[] this[int index] => _map[index];
        private int NorthBorder { get; set; }
        private int SouthBorder { get; set; }
        private int EastBorder { get; set; }
        private int WestBorder { get; set; }
        
        public ElevationMap(List<string> inputs)
        {
            var maxY = inputs.Count - 1;
            _map = inputs.Select((s, y) => {
                return s.ToCharArray().Select((c, x) =>
                {
                    if (c == 'S')
                    {
                        _start = new RouteLocation(x, y, 1);
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
        
        private List<RouteLocation> GetNeighbors(RouteLocation current)
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
            neighbors.RemoveAll(n => current.Elevation - 1 > n.Elevation);
            return neighbors.ToList();
        }

        public List<RouteLocation> GetBestPathFromStartLocation()
        {
            _bestPath = BFS<RouteLocation>.Search(_destination, GetNeighbors,
                (RouteLocation current) => current.ToString() == _start.ToString());
            return _bestPath;
        }
        
        public List<RouteLocation> GetBestOverallPath()
        {
            _bestPath = BFS<RouteLocation>.Search(_destination, GetNeighbors, current => current.Elevation == 1);
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

    private class RouteLocation : IEquatable<RouteLocation>
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

        public bool Equals(RouteLocation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RouteLocation)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}