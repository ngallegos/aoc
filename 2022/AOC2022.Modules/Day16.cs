using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2022.Modules;

public class Day16 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var sampleValves = get_sample().Select(x => new Valve(x)).ToList();
        foreach (var valve in sampleValves)
        {
            valve.LinkValveTunnels(sampleValves);
        }
        
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
    
    private class Valve
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
    }
}