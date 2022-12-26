

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day15 : DayBase
{
    public override bool Ignore => false;
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
            ActualFrequency = actualGrid.IsolateDistressBeacon(4000000)
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

        
        // Below had help from:
        // https://github.com/micka190/advent-of-code/blob/main/2022/day%2015/Solution/Solver.cs
        // https://github.com/schovanec/AdventOfCode/blob/4fe04dc4628cc6bcc1eab502bf3469dbe0317dd8/2022/Day15/Program.cs#L76
        // last wrong answer: 796961409
        public long IsolateDistressBeacon(int upperLimit)
        {
            var limit = new Range(0, upperLimit);

            for (var y = 0; y <= upperLimit; ++y)
            {
                var covered = _sensors.Select(s => s.GetSlice(y));
                var gap = FindGap(covered, limit);
                if (gap is int x)
                    return (x * 4000000L) + y;
            }

            return 0;
        }
        
        static int? FindGap(IEnumerable<Range> ranges, Range limit)
        {
            var ordered = ranges.Select(r => r.Intersect(limit))
                .Where(r => !r.IsEmpty)
                .OrderBy(r => r.Min)
                .ThenBy(r => r.Max);

            int max = limit.Min - 1;
            foreach (var r in ordered)
            {
                if (max + 1 < r.Min)
                    return max + 1;

                max = Math.Max(max, r.Max);
            }

            return max < limit.Max
                ? max + 1
                : null;
        }

        private List<Coordinate> GetEdgeCoordinates(Sensor sensor)
        {
            var left = new Coordinate(sensor.X - sensor.DistanceToBeacon - 1, sensor.Y);
            var right = new Coordinate(sensor.X + sensor.DistanceToBeacon + 1, sensor.Y);
            var top = new Coordinate(sensor.X, sensor.Y + sensor.DistanceToBeacon + 1);
            var bottom = new Coordinate(sensor.X, sensor.Y - sensor.DistanceToBeacon - 1);

            var edges = new List<Coordinate>();

            edges = edges
                .Concat(GetCoordinatesBetween(left, top))
                .Concat(GetCoordinatesBetween(bottom, right))
                .Concat(GetCoordinatesBetween(top, right))
                .Concat(GetCoordinatesBetween(left, bottom))
                .ToList();

            return edges;
        }
        
        private static IEnumerable<Coordinate> GetCoordinatesBetween(Coordinate start, Coordinate end)
        {
            var (left, right) = start.X <= end.X ? (start, end) : (end, start);
            var coordinates = new List<Coordinate>();

            if (left.Y <= end.Y)
            {
                for (var offset = 0; offset <= right.X - left.X; ++offset)
                {
                    coordinates.Add(new Coordinate(left.X + offset, left.Y + offset));
                }
            }
            else
            {
                for (var offset = 0; offset <= right.X - left.X; ++offset)
                {
                    coordinates.Add(new Coordinate(left.X + offset, left.Y - offset));
                }
            }

            return coordinates;
        }
    }

    record struct Range(int Min, int Max)
    {
        public static Range Empty = new(0, -1);

        public bool IsEmpty => Min > Max;

        public IEnumerable<int> Values
            => IsEmpty ? Enumerable.Empty<int>() : Enumerable.Range(Min, Max - Min + 1);

        public bool Overlaps(Range other)
            => !IsEmpty
               && !other.IsEmpty
               && Min <= other.Max
               && Max >= other.Min;

        public Range Intersect(Range other)
            => Overlaps(other) ? new(Math.Max(Min, other.Min), Math.Min(Max, other.Max)) : Empty;
    }
    private class Sensor : Coordinate<char>
    {
        private static Regex _locationRegex = new Regex(@"x=(?<x>-?\d*), y=(?<y>-?\d*)");

        public int BeaconDistance => DistanceTo(ClosestBeacon.X, ClosestBeacon.Y);
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
        public Range GetSlice(int y)
        {
            var dy = Math.Abs(y - Y);
            if (dy > BeaconDistance)
                return Range.Empty;

            var dx = BeaconDistance - dy;
            return new(X - dx, X + dx);
        }
        public Sensor(int x, int y, char value, Beacon closestBeacon) : base(x, y, value)
        {
            ClosestBeacon = closestBeacon;
            DistanceToBeacon = DistanceTo(closestBeacon.X, closestBeacon.Y);
        }

        public int DistanceTo(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2);
        }

        public bool LocationWithinManhattanDistance(int x, int y)
        {
            var d = DistanceTo(x, y);
            return d <= DistanceToBeacon;
        }
        
        public Beacon ClosestBeacon { get; private set; }
        public int DistanceToBeacon { get; private set; }
    }
    
    private class Beacon : Coordinate<char>
    {
        public Beacon(int x, int y, char value) : base(x, y, value)
        {
        }
    }
    
}