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
        var grid = new SensorGrid(sampleSensors);

        return grid.Render();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class SensorGrid : Grid<char>
    {
        public SensorGrid(List<Sensor> sensors)
        {
            var sensorsAndBeacons = sensors.Cast<GridLocation<char>>()
                .Concat(sensors.Select(x => x.ClosestBeacon)).ToList();
            var minX = sensorsAndBeacons.Min(x => x.X);
            var maxX = sensorsAndBeacons.Max(x => x.X);
            var minY = sensorsAndBeacons.Min(x => x.Y);
            var maxY = sensorsAndBeacons.Max(x => x.Y);
            
            Initialize(new GridLocation(minX, minY), new GridLocation(maxX, maxY), sensorsAndBeacons, '.');
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
            ManhattanDistance = Math.Abs(x - closestBeacon.X) + Math.Abs(y - closestBeacon.Y);
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