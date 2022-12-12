using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day12 : DayBase
{
    public override bool Completed { get; }
    private static List<(char letter, int height)> HeightMap = "abcdefghijklmnopqrstuvwxyz"
        .ToCharArray()
        .Select((c, i) => (c, i+1)).ToList();

    private ElevationMap _sampleMap;
    private ElevationMap _actualMap;

    public Day12()
    {
        _sampleMap = new ElevationMap(get_sample("Part1").ToList());
        _actualMap = new ElevationMap(get_input("Part1").ToList());
    }
    
    public override dynamic Part1()
    {
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class ElevationMap
    {
        private readonly int[][] _map;
        private MapLocation _start;
        private MapLocation _end;

        public ElevationMap(List<string> inputs)
        {
            var maxY = inputs.Count - 1;
            _map = inputs.Select((s, y) => {
                return s.ToCharArray().Select((c, x) =>
                {
                    if (c == 'S')
                    {
                        _start = new MapLocation(x, maxY - y);
                        return 1;
                    }

                    if (c == 'E')
                    {
                        _end = new MapLocation(x, maxY - y);
                        return 1;
                    }
                    return int.Parse(HeightMap.First(h => h.letter == c).height.ToString());
                }).ToArray(); }).ToArray();
        }
    }
    
    private class MapLocation
    {
        public MapLocation(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }
        public int XCoordinate { get; private set; }
        public int YCoordinate { get; private set; }

        public override string ToString()
        {
            return $"({XCoordinate},{YCoordinate})";
        }
    }
}