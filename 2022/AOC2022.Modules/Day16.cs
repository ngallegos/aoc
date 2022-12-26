using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2022.Modules;

public class Day16 : DayBase
{
    // https://www.reddit.com/r/adventofcode/comments/zn6k1l/2022_day_16_solutions/
    public override bool Ignore => false;
    public override dynamic Part1()
    {
        var sampleValves = get_sample().Select(x => new Valve(x)).ToList();
        foreach (var valve in sampleValves)
        {
            valve.LinkValveTunnels(sampleValves);
        }
        
        // TODO bfs on all valves with nonzero flow rate and start with max flow rates first
        
        return sampleValves;
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private enum ValveState
    {
        Open,
        Closed
    }

    record TunnelMap(int[,] distances, Valve[] valves);
    
    private class Valve : IEquatable<Valve>
    {
        
        private static Regex _valveRegex = new Regex(@"^Valve (?<name>\w\w) has flow rate=(?<flowRate>\d*); tunnels? leads? to valves? (?<tunnels>.*)$");
        public string Name { get; }
        public int FlowRate { get; }
        public int MinutesOpened { get; private set; }
        public ValveState State { get; private set; } = ValveState.Closed;
        public int TotalPressureRelease => MinutesOpened * FlowRate;
        public List<string> _tunnelDestinationNames = new List<string>();
        private List<Valve> TunnelDestinations { get; set; } = new List<Valve>();

        public Valve(string input)
        {
            var match = _valveRegex.Match(input);
            Name = match.Groups["name"].Value;
            FlowRate = int.Parse(match.Groups["flowRate"].Value);
            _tunnelDestinationNames = match.Groups["tunnels"].Value.Split(", ").ToList();
        }

        public void LinkValveTunnels(List<Valve> allValves)
        {
            var relevantValves = allValves.Where(v => _tunnelDestinationNames.Contains(v.Name));
            TunnelDestinations.AddRange(relevantValves);
        }

        public void OpenValve()
        {
            State = ValveState.Open;
        }

        public void MinuteElapsed()
        {
            if (State == ValveState.Open)
                MinutesOpened++;
        }

        public bool Equals(Valve other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Valve)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}