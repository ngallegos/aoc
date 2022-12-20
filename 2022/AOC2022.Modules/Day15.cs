using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day15 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var sampleSensors = get_sample()
            .Select(Sensor.Parse).ToList();
        var actualSensors = get_input().Select(Sensor.Parse).ToList();
        var sampleGrid = new SensorGrid(sampleSensors);
        var actualGrid = new SensorGrid(actualSensors);

        return new
        {
            SampleEliminatedPositions = sampleGrid.GetEliminatedPositions(10),
            //ActualEliminatedPositions = actualGrid.GetEliminatedPositions(2000000)
        };
    }

    /// <summary>
    /// https://www.reddit.com/r/adventofcode/comments/zmcn64/2022_day_15_solutions/
    /// </summary>
    /// <returns></returns>
    public override dynamic Part2()
    {
        var sampleSensors = get_sample()
            .Select(Sensor.Parse).ToList();
        var actualSensors = get_input().Select(Sensor.Parse).ToList();
        var sampleGrid = new SensorGrid(sampleSensors);
        var actualGrid = new SensorGrid(actualSensors);
        return new
        {
            SampleFrequency = sampleGrid.IsolateDistressBeacon(20),
            //ActualFrequency = actualGrid.IsolateDistressBeacon(4000000)
        };
    }

    private class SensorGrid
    {
        private List<Sensor> _sensors;
        public SensorGrid(List<Sensor> sensors)
        {
            _sensors = sensors;
        }

        public long GetEliminatedPositions(int row, int? minX = null, int? maxX = null)
        {
            var beacons = _sensors.Select(x => x.ClosestBeacon)
                .ToList();
            beacons.Sort((left, right) => left.X.CompareTo(right.X));
            _sensors.Sort((left, right) => left.X.CompareTo(right.X));
            
            minX ??= _sensors.Min(x => x.X - x.DistanceToBeacon);
            maxX ??= _sensors.Max(x => x.X + x.DistanceToBeacon);

            var rangeStart = Math.Min(minX.Value, beacons[0].X);
            var rangeEnd = Math.Max(maxX.Value, beacons[^1].X);
            var eliminatedPositions = 0;

            for (var x = rangeStart; x < rangeEnd; ++x)
            {
                var positionString = $"({x},{row})";
                if (_sensors.Any(sensor => sensor.LocationWithinManhattanDistance(x, row) &&  positionString != sensor.ClosestBeacon.ToString()))
                    ++eliminatedPositions;
            }

            return eliminatedPositions;
        }

        private int CalculateTuningFrequency(int x, int y)
        {
            return (x * 4000000) + y;
        }
        
        // Below had help from:
        // https://github.com/micka190/advent-of-code/blob/main/2022/day%2015/Solution/Solver.cs
        public int IsolateDistressBeacon(int max)
        {
            var minX = 0;
            var minY = 0;
            var edgeLocations = _sensors
                .Select(GetEdgeLocations)
                .SelectMany(location => location).ToList();

            foreach (var location in edgeLocations)
            {
                if (location.X > minX && location.X < max
                                      && location.Y > minY && location.Y < max)
                {
                    var inSensorRange = _sensors.Any(s =>
                        s.DistanceToBeacon >= s.CalculateManhattanDistance(location.X, location.Y));
                    if (!inSensorRange)
                        return CalculateTuningFrequency(location.X, location.Y);
                }
            }

            return -1;
        }

        private List<GridLocation> GetEdgeLocations(Sensor sensor)
        {
            var left = new GridLocation(sensor.X - sensor.DistanceToBeacon - 1, sensor.Y);
            var right = new GridLocation(sensor.X + sensor.DistanceToBeacon + 1, sensor.Y);
            var top = new GridLocation(sensor.X, sensor.Y + sensor.DistanceToBeacon + 1);
            var bottom = new GridLocation(sensor.X, sensor.Y - sensor.DistanceToBeacon - 1);

            var edges = new List<GridLocation>();

            edges = edges
                .Concat(GetLocationsBetween(left, top))
                .Concat(GetLocationsBetween(bottom, right))
                .Concat(GetLocationsBetween(top, right))
                .Concat(GetLocationsBetween(left, bottom))
                .ToList();

            return edges;
        }
        
        private List<GridLocation> GetLocationsBetween(GridLocation start, GridLocation end)
        {
            var (left, right) = start.X <= end.X ? (start, end) : (end, start);
            var locations = new List<GridLocation>();

            if (left.Y <= end.Y)
            {
                for (var offset = 0; offset <= right.X - left.X; ++offset)
                {
                    locations.Add(new GridLocation(left.X + offset, left.Y + offset));
                }
            }
            else
            {
                for (var offset = 0; offset <= right.X - left.X; ++offset)
                {
                    locations.Add(new GridLocation(left.X + offset, left.Y - offset));
                }
            }

            return locations;
        }
    }

    private class Sensor : GridLocation<char>
    {
        private static Regex _locationRegex = new Regex(@"x=(?<x>-?\d*), y=(?<y>-?\d*)");

        public static Sensor Parse(string positionReport)
        {
            var parts = positionReport.Split(':');
            var sensorMatch = _locationRegex.Match(parts[0]);
            var beaconMatch = _locationRegex.Match(parts[1]);
            var beacon = new Beacon(int.Parse(beaconMatch.Groups["x"].Value), int.Parse(beaconMatch.Groups["y"].Value), 'B');
            var sensor = new Sensor(int.Parse(sensorMatch.Groups["x"].Value), int.Parse(sensorMatch.Groups["y"].Value),
                'S', beacon);
            return sensor;
        }
        
        public Sensor(int x, int y, char value, Beacon closestBeacon) : base(x, y, value)
        {
            ClosestBeacon = closestBeacon;
            DistanceToBeacon = CalculateManhattanDistance(closestBeacon.X, closestBeacon.Y);
        }

        public int CalculateManhattanDistance(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2) + 1;
        }

        public bool LocationWithinManhattanDistance(int x, int y)
        {
            var d = CalculateManhattanDistance(x, y);
            return d <= DistanceToBeacon;
        }
        
        public Beacon ClosestBeacon { get; private set; }
        public int DistanceToBeacon { get; private set; }
    }
    
    private class Beacon : GridLocation<char>
    {
        public Beacon(int x, int y, char value) : base(x, y, value)
        {
        }
    }
    
}