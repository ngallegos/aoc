using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day14 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var sampleDiagram = new CaveDiagram(get_sample().ToList());
        var actualDiagram = new CaveDiagram(get_input().ToList());
        sampleDiagram.ReleaseTheSandIntoTheAbyss();
        actualDiagram.ReleaseTheSandIntoTheAbyss();
        return new
        {
            sampleDiagram,
            actualDiagram
        };
    }

    public override dynamic Part2()
    {
        var sampleDiagram = new CaveDiagram(get_sample().ToList());
        var actualDiagram = new CaveDiagram(get_input().ToList());
        sampleDiagram.ReleaseTheSandUntilItStops();
        actualDiagram.ReleaseTheSandUntilItStops();
        return new
        {
            sampleDiagram,
            actualDiagram
        };
    }

    private class CaveDiagram
    {
        private List<string> _diagram;
        public string this[int index] => _diagram[index];
        public int Length => _diagram.Count;
        public int _rockPathXMin;
        public int _rockPathXMax;
        public int _xMin;
        private int _xMax;
        private int _yMin;
        private int _yMax;
        private int _unitsOfSandDropped = 0;
        public int UnitsOfSandDropped => _unitsOfSandDropped;
        public List<string> Diagram => _diagram;
        private List<List<ScanLocation>> _rockPaths = new List<List<ScanLocation>>();

        public CaveDiagram(List<string> inputs)
        {
            _rockPaths = inputs.Select(x => x.Split(" -> ")
                .Select(l =>
                {
                    var lParts = l.Split(',');
                    return new ScanLocation(int.Parse(lParts[0]), int.Parse(lParts[1]));
                }).ToList()).ToList();
        }

        private string GetNewRow(char seedCharacter = '.')
        {
            return new string(Enumerable.Range(0, _xMax - _xMin + 1).Select(_ => seedCharacter).ToArray());
        }

        private void UpdateBoundaries()
        {
            var rockPathWidth = _rockPathXMax - _rockPathXMin + 1;
            var extraWidth = _diagram[0].Length - rockPathWidth;
            _xMin = _rockPathXMin - extraWidth/2;
            _xMax = _rockPathXMax + extraWidth/2;
            _yMin = 0;
            _yMax = _diagram.Count - 1;
        }
        
        private void PopulateDiagram()
        {
            var allPaths = _rockPaths.SelectMany(x => x).ToList();
            _xMin = _rockPathXMin = allPaths.Min(x => x.XCoordinate);
            _xMax = _rockPathXMax = allPaths.Max(x => x.XCoordinate);
            _yMin = 0;
            _yMax = allPaths.Max(x => x.YCoordinate);
            _diagram = Enumerable.Range(0, _yMax+1)
                .Select(x => GetNewRow()).ToList();

            SetCharacter(500, 0, '+');
            
            foreach (var rockPath in _rockPaths)
            {
                for (int i = 1; i < rockPath.Count; i++)
                {
                    var pointA = rockPath[i - 1];
                    var pointB = rockPath[i];
                    if (pointA.XCoordinate == pointB.XCoordinate)
                    {
                        var currentY = pointA.YCoordinate;
                        var direction = pointB.YCoordinate > pointA.YCoordinate ? 1 : -1;
                        while (currentY != pointB.YCoordinate)
                        {
                            SetCharacter(pointB.XCoordinate, currentY, '#');
                            currentY += direction;
                        }
                        SetCharacter(pointB.XCoordinate, currentY, '#');
                    } 
                    else if (pointA.YCoordinate == pointB.YCoordinate)
                    {
                        var currentX = pointA.XCoordinate;
                        var direction = pointB.XCoordinate > pointA.XCoordinate ? 1 : -1;
                        while (currentX != pointB.XCoordinate)
                        {
                            SetCharacter(currentX, pointB.YCoordinate, '#');
                            currentX += direction;
                        }
                        SetCharacter(currentX, pointB.YCoordinate, '#');
                    }
                }
            }
        }
        public void SetCharacter(int x, int y, char c)
        {
            var xCoordinate = x - _xMin;
            var row = _diagram[y];
            row = row.Remove(xCoordinate, 1).Insert(xCoordinate, c.ToString());
            _diagram[y] = row;
        }

        public char GetLocationValue(ScanLocation location)
        {
            return _diagram[location.YCoordinate][location.XCoordinate - _xMin];
        }

        public bool InBounds(ScanLocation location)
        {
            return XCoordinateInBounds(location) && YCoordinateInBounds(location);
        }

        private bool XCoordinateInBounds(ScanLocation location)
        {
            return location.XCoordinate >= _xMin
                   && location.XCoordinate <= _xMax;
        }
        
        private bool YCoordinateInBounds(ScanLocation location)
        {
            return location.YCoordinate >= _yMin
                   && location.YCoordinate <= _yMax;
        }
        
        private SandUnit DropUnitOfSand()
        {
            var sand = new SandUnit();
            sand.Fall(this);
            return sand;
        }

        public void ReleaseTheSandIntoTheAbyss()
        {
            PopulateDiagram();
            var unitsOfSandDropped = 0;
            var sand = DropUnitOfSand();
            while (InBounds(sand))
            {
                sand = DropUnitOfSand();
                unitsOfSandDropped++;
            }

            _unitsOfSandDropped = unitsOfSandDropped;
        }

        public void ReleaseTheSandUntilItStops()
        {
            PopulateDiagram();
            _diagram.Add(GetNewRow());
            _diagram.Add(GetNewRow('#'));
            UpdateBoundaries();
            var unitsOfSandDropped = 1;
            var sandOrigin = new SandUnit();
            var sand = DropUnitOfSand();
            while (InBounds(sand) && sandOrigin.ToString() != sand.ToString())
            {
                sand = DropUnitOfSand();
                if (!InBounds(sand) && !XCoordinateInBounds(sand))
                {
                    // get wider
                    for (int y = 0; y <= _yMax; y++)
                    {
                        var seedChar = '.';
                        if (y == _yMax)
                            seedChar = '#'; // floor
                        _diagram[y] = $"{seedChar}{_diagram[y]}{seedChar}";
                    }
                    UpdateBoundaries();
                } else 
                    unitsOfSandDropped++;
            }

            _unitsOfSandDropped = unitsOfSandDropped;
        }
    }

    private class ScanLocation
    {
        public ScanLocation(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }
        public int XCoordinate { get; protected set; }
        public int YCoordinate { get; protected set; }

        public override string ToString()
        {
            return $"({XCoordinate},{YCoordinate})";
        }
    }

    private class SandUnit : ScanLocation
    {
        public SandUnit() : base(500, 0)
        {
        }

        public void Fall(CaveDiagram diagram)
        {
            var nextLocation = new ScanLocation(XCoordinate, YCoordinate);
            while (CanKeepFalling(nextLocation) && diagram.InBounds(nextLocation))
            {
                XCoordinate = nextLocation.XCoordinate;
                YCoordinate = nextLocation.YCoordinate;
                nextLocation = GetNextLocation(diagram);
            }
            if (nextLocation != null)
            {
                XCoordinate = nextLocation.XCoordinate;
                YCoordinate = nextLocation.YCoordinate;
            }
            if (diagram.InBounds(this))
                diagram.SetCharacter(XCoordinate, YCoordinate, 'o');
        }

        private bool CanKeepFalling(ScanLocation location)
        {
            return location != null;
        }
        
        private ScanLocation GetNextLocation(CaveDiagram diagram)
        {
            var below = BelowMe(diagram);
            if (!below.Any())
                return new ScanLocation(XCoordinate, YCoordinate + 1);
            var sandOrRock = new[] { '#', 'o' };
            var directlyBeneathIndex = below.FindIndex(x => x.XCoordinate == XCoordinate);
            var directlyBeneath = below[directlyBeneathIndex];
            if (sandOrRock.Contains(diagram.GetLocationValue(directlyBeneath)))
            {
                if (directlyBeneathIndex == 0)
                    return new ScanLocation(XCoordinate - 1, YCoordinate + 1); // falling left means the abyss
                if (below.Count == 2)
                    return new ScanLocation(XCoordinate + 1, YCoordinate + 1); // falling right means the abyss
                var left = below[directlyBeneathIndex - 1];
                var right = below[directlyBeneathIndex + 1];
                if (!sandOrRock.Contains(diagram.GetLocationValue(left)))
                    return new ScanLocation(XCoordinate - 1, YCoordinate + 1);
                if (!sandOrRock.Contains(diagram.GetLocationValue(right)))
                    return new ScanLocation(XCoordinate + 1, YCoordinate + 1);
                return null;
            }

            return new ScanLocation(XCoordinate, YCoordinate + 1);
        }

        private List<ScanLocation> BelowMe(CaveDiagram diagram)
        {
            var locations = new List<ScanLocation>();
            if (YCoordinate == diagram.Length)
                return locations;

            var xIndex = XCoordinate - diagram._xMin;
            if (xIndex > 0)
                locations.Add(new ScanLocation(XCoordinate - 1, YCoordinate + 1));
            locations.Add(new ScanLocation(XCoordinate, YCoordinate + 1));
            if (xIndex < diagram[0].Length - 1)
                locations.Add(new ScanLocation(XCoordinate + 1, YCoordinate + 1));
            return locations;
        }
    }
}