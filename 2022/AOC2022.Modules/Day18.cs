using System;
using System.Collections.Generic;
using System.Linq;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day18 : DayBase
{
    public override bool Ignore => true;

    private LavaDroplet _sampleDroplet;
    private LavaDroplet _actualDroplet;
    
    public override dynamic Part1()
    {
        _sampleDroplet ??= new LavaDroplet(get_sample());
        _actualDroplet ??= new LavaDroplet(get_input());

        _sampleDroplet.AnalyzePart1();
        _actualDroplet.AnalyzePart1();
        
        return new
        {
            sample = _sampleDroplet.SurfaceArea,
            actual = _actualDroplet.SurfaceArea
        };
    }

    public override dynamic Part2()
    {
        _sampleDroplet ??= new LavaDroplet(get_sample());
        _actualDroplet ??= new LavaDroplet(get_input());
        
        _sampleDroplet.AnalyzePart2();
        _actualDroplet.AnalyzePart2();
        return new
        {
            sample = _sampleDroplet.ExternalSurfaceArea,
            actual = _actualDroplet.ExternalSurfaceArea
        };
    }

    private class LavaDroplet
    {
        private List<Cube> _cubes;
        private int _xMin;
        private int _xMax;
        private int _yMin;
        private int _yMax;
        private int _zMin;
        private int _zMax;

        public int SurfaceArea => _cubes.Sum(c => c.UnconnectedSides);
        private int _externalSurfaceArea = 0;
        public int ExternalSurfaceArea => _externalSurfaceArea;

        public LavaDroplet(IEnumerable<string> input)
        {
            _cubes = input.Select(x => new Cube(x))
                .ToList();;
            _xMin = _cubes.Min(c => c.X);
            _xMax = _cubes.Max(c => c.X);
            _yMin = _cubes.Min(c => c.Y);
            _yMax = _cubes.Max(c => c.Y);
            _zMin = _cubes.Min(c => c.Z);
            _zMax = _cubes.Max(c => c.Z);
        }

        public void AnalyzePart1()
        {
            _cubes.ForEach(x => x.FindConnections(_cubes.Where(c => c != x && !c.Analyzed).ToList()));
        }
        
        public void AnalyzePart2()
        {
            var xRange = Enumerable.Range(_xMin, _xMax + 1).ToList();
            var yRange = Enumerable.Range(_yMin, _yMax + 1).ToList();
            var zRange = Enumerable.Range(_zMin, _zMax + 1).ToList();
            var _cubeRecords = _cubes.Select(c => new Cube2(c.X, c.Y, c.Z));
            _externalSurfaceArea = 0;
            
            bool IsOutside(Cube2 cube)
            {
                if (_cubeRecords.Contains(cube)) return false;

                List<Cube2> GetNeighbors(Cube2 c)
                {
                    return _neighbors.Select(n => new Cube2(c.X + n.X, c.Y + n.Y, c.Z + n.Z)).ToList();
                }

                bool IsOutsideAnyRange(Cube2 c)
                {
                    return (!xRange.Contains(c.X) || !yRange.Contains(c.Y) || !zRange.Contains(c.Z));
                }

                var bfsResult = BFS<Cube2>.Search(cube, GetNeighbors, IsOutsideAnyRange);

                return bfsResult != null;
            }

            foreach (var cube in _cubeRecords)
            {
                foreach (var neighbor in _neighbors)
                    if (IsOutside(new Cube2(cube.X + neighbor.X, cube.Y + neighbor.Y, cube.Z + neighbor.Z)))
                        _externalSurfaceArea++;
            }
        }
        
        

        private List<Cube2> _neighbors = new List<Cube2>
        {
            new Cube2(1, 0, 0),
            new Cube2(-1, 0, 0),
            new Cube2(0, 1, 0),
            new Cube2(0, -1, 0),
            new Cube2(0, 0, 1),
            new Cube2(0, 0, -1),
        };
    }

    private record Cube2(int X, int Y, int Z);
    
    private class Cube
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        
        public bool Analyzed { get; private set; }

        private List<Cube> XLavaNeighbors { get; } = new List<Cube>();
        private List<Cube> YLavaNeighbors { get; } = new List<Cube>();
        private List<Cube> ZLavaNeighbors { get; } = new List<Cube>();

        public int UnconnectedSides => 6 - XLavaNeighbors.Count - YLavaNeighbors.Count - ZLavaNeighbors.Count;
        public int ExternalSides => UnconnectedSides - _internalSides;
        private int _internalSides = 0;

        public Cube(string input)
        {
            var coordinates = input.Split(',');
            X = int.Parse(coordinates[0]);
            Y = int.Parse(coordinates[1]);
            Z = int.Parse(coordinates[2]);
        }
        
        public void FindConnections(List<Cube> cubes)
        {
            foreach (var cube in cubes)
            {
                if (cube.Analyzed)
                    continue;
                if ((cube.X == X - 1 || cube.X == X + 1) && cube.Y == Y && cube.Z == Z)
                {
                    XLavaNeighbors.Add(cube);
                    cube.XLavaNeighbors.Add(this);
                } else if ((cube.Y == Y - 1 || cube.Y == Y + 1) && cube.X == X && cube.Z == Z)
                {
                    YLavaNeighbors.Add(cube);
                    cube.YLavaNeighbors.Add(this);
                } else if ((cube.Z == Z - 1 || cube.Z == Z + 1) && cube.Y == Y && cube.X == X)
                {
                    ZLavaNeighbors.Add(cube);
                    cube.ZLavaNeighbors.Add(this);
                }
            }

            // if (XLavaNeighbors.All(x => x.X != X - 1) && cubes.Any(c => c.X < X - 1))
            //     _internalSides++;
            // else if (XLavaNeighbors.All(x => x.X != X + 1) && cubes.Any(c => c.X > X + 1))
            //     _internalSides++;
            //
            // if (YLavaNeighbors.All(x => x.Y != Y - 1) && cubes.Any(c => c.Y < Y - 1))
            //     _internalSides++;
            // else if (YLavaNeighbors.All(x => x.Y != Y + 1) && cubes.Any(c => c.Y > Y + 1))
            //     _internalSides++;
            //
            // if (ZLavaNeighbors.All(x => x.Z != Z - 1) && cubes.Any(c => c.Z < Z - 1))
            //     _internalSides++;
            // else if (ZLavaNeighbors.All(x => x.Z != Z + 1) && cubes.Any(c => c.Z < Z + 1))
            //     _internalSides++;

            Analyzed = true;
        }
        
    }
}