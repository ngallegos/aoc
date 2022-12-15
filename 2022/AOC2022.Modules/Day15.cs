using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day15 : DayBase
{
    public override bool Completed { get; }
    public override dynamic Part1()
    {
        var sampleSensors = get_sample()
            .Select(Sensor.Parse).ToList();
        var actualSensors = get_input().Select(Sensor.Parse).ToList();
        var sampleGrid = new SensorGrid(sampleSensors);
        var actualGrid = new SensorGrid(actualSensors);

        return new
        {
            SampleEliminatedPositions = sampleGrid.GetEliminatedPositions(10).eliminatedPositions.Count,
            //ActualEliminatedPositiongs = actualGrid.GetEliminatedPositions(2000000).eliminatedPositions.Count
        };
    }

    public override dynamic Part2()
    {
        var sampleSensors = get_sample()
            .Select(Sensor.Parse).ToList();
        var actualSensors = get_input().Select(Sensor.Parse).ToList();
        var sampleGrid = new SensorGrid(sampleSensors);
        var actualGrid = new SensorGrid(actualSensors);

        return new
        {
            SampleFrequency = sampleGrid.IsolateDistressBeacon(20, 20),
            //ActualEliminatedPositiongs = actualGrid.GetEliminatedPositions(2000000).eliminatedPositions.Count
        };
    }

    private class SensorGrid
    {
        private List<Sensor> _sensors;
        public SensorGrid(List<Sensor> sensors)
        {
            _sensors = sensors;
        }

        public (List<(int x, int y)> eliminatedPositions, int lastPotentialX) GetEliminatedPositions(int row, int? minX = null, int? maxX = null)
        {
            minX ??= _sensors.Min(x => x.X - x.ManhattanDistance);
            maxX ??= _sensors.Max(x => x.X + x.ManhattanDistance);
            var eliminatedPositions = new List<(int x, int y)>();
            var beaconLocations = _sensors.Select(x => x.ClosestBeacon.ToString()).ToList();
            var lastPotentialX = minX.Value;
            for (int x = minX.Value; x <= maxX.Value; x++)
            {
                var eliminationFound = false;
                foreach (var sensor in _sensors)
                {
                    var positionString = $"({x},{row})";
                    if (sensor.LocationWithinManhattanDistance(x, row) && !beaconLocations.Contains(positionString))
                    {
                        eliminationFound = true;
                        eliminatedPositions.Add((x, row));
                        break;
                    }
                }
                if (!eliminationFound)
                    lastPotentialX = x;
            }
            return (eliminatedPositions.DistinctBy(item => $"({item.x},{item.y})").ToList(), lastPotentialX);
        }

        public int IsolateDistressBeacon(int maxX, int maxY)
        {
            var minX = 0;
            var minY = 0;
            for (int y = minY; y <= maxY; y++)
            {
                var results = GetEliminatedPositions(y, minX, maxX);
                Console.WriteLine(results.eliminatedPositions.Count);
                if (results.eliminatedPositions.Count <= maxX)
                {
                    return CalculateTuningFrequency(results.lastPotentialX, y);
                }
            }

            return -1;
        }

        private int CalculateTuningFrequency(int x, int y)
        {
            return (x * 4000000) + y;
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
            ManhattanDistance = CalculateManhattanDistance(closestBeacon.X, closestBeacon.Y);
        }

        private int CalculateManhattanDistance(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2) + 1;
        }

        public bool LocationWithinManhattanDistance(int x, int y)
        {
            var d = CalculateManhattanDistance(x, y);
            return d <= ManhattanDistance;
        }
        
        public Beacon ClosestBeacon { get; private set; }
        public int ManhattanDistance { get; private set; }
    }
    
    private class Beacon : GridLocation<char>
    {
        public Beacon(int x, int y, char value) : base(x, y, value)
        {
        }
    }
    
}