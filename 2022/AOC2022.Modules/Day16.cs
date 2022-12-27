using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day16 : DayBase
{
    // https://www.reddit.com/r/adventofcode/comments/zn6k1l/2022_day_16_solutions/
    public override bool Ignore => false;
    public override dynamic Part1()
    {
        return "ignored";
        var valves = get_input().Select(x => new Valve(x)).ToList();
        foreach (var valve in valves)
        {
            valve.LinkValveTunnels(valves);
        }

        // help from : https://www.reddit.com/r/adventofcode/comments/zn6k1l/comment/j0yvgu8/?utm_source=share&utm_medium=web2x&context=3
        var distances = new List<(string startValve, string endValve, int distance)>();
        
        // Compute shortest distance between points
        foreach (var startValve in valves)
        {
            foreach (var endValve in valves)
            {
                var bfsResult =
                BFS<Valve>.Search(startValve, 
                    v => v.TunnelDestinations, 
                    v => v.Name == endValve.Name);
                distances.Add((startValve.Name, endValve.Name, bfsResult.Count - 1));
            }
        }
        
        int max = 0;
        var curPath = new List<Valve>();
        var paths = new Dictionary<HashSet<Valve>, int>();
        int available = valves.Count(v => v.FlowRate > 0 && !v.IsOpen);
        
        backtrack(valves, distances, 30, valves.First(x => x.Name == "AA"), 
            0, 0, available, ref max, curPath, paths);
        
        return new
        {
            max
        };
    }
    
    public override dynamic Part2()
    {
        var valves = get_input().Select(x => new Valve(x)).ToList();
        foreach (var valve in valves)
        {
            valve.LinkValveTunnels(valves);
        }

        // help from : https://www.reddit.com/r/adventofcode/comments/zn6k1l/comment/j0yvgu8/?utm_source=share&utm_medium=web2x&context=3
        var distances = new List<(string startValve, string endValve, int distance)>();
        
        // Compute shortest distance between points
        foreach (var startValve in valves)
        {
            foreach (var endValve in valves)
            {
                var bfsResult =
                    BFS<Valve>.Search(startValve, 
                        v => v.TunnelDestinations, 
                        v => v.Name == endValve.Name);
                distances.Add((startValve.Name, endValve.Name, bfsResult.Count - 1));
            }
        }
        
        int max = 0;
        var curPath = new List<Valve>();
        var paths = new Dictionary<HashSet<Valve>, int>();
        int available = valves.Count(v => v.FlowRate > 0 && !v.IsOpen);
        
        backtrack(valves, distances, 26, valves.First(x => x.Name == "AA"), 
            0, 0, available, ref max, curPath, paths);
        // Part 2
        foreach(var a in paths.Keys.OrderByDescending(o => paths[o]))
        {
            foreach (var b in paths.Keys.OrderByDescending(o => paths[o]))
            {
                if (paths[a] + paths[b] < max)
                    goto exit;

                if(!a.Overlaps(b))
                {
                    max = paths[a] + paths[b];
                }
            }
        }
        exit:

        Console.WriteLine(max);
        return new
        {
            max
        };
    }

    
    // help from : https://www.reddit.com/r/adventofcode/comments/zn6k1l/comment/j0yvgu8/?utm_source=share&utm_medium=web2x&context=3
    void backtrack(List<Valve> valves, List<(string startValve, string endValve, int distance)> dist, int time, Valve cur, 
        int pressure, int cumulpressure,
        int available, ref int max, List<Valve> curPath, Dictionary<HashSet<Valve>, int> paths)
    {
        if (time == 0 || available == 0)
        {
            max = Math.Max(max, cumulpressure + (pressure * time));
            return;
        }

        paths.Add(new HashSet<Valve>(curPath), cumulpressure + (pressure * time));

        foreach (var n in valves)
        {
            if (n == cur  || n.IsOpen || n.FlowRate == 0)
                continue;

            var ttg = dist.First(x => x.startValve == n.Name && x.endValve == cur.Name).distance + 1;
            if(ttg > time)
            {
                ttg = time;
            }
            n.IsOpen = true;
            curPath.Add(n);
            backtrack(valves, dist, time - ttg, n, pressure + n.FlowRate, cumulpressure + (pressure * ttg), available - 1, ref max, curPath, paths);
            curPath.RemoveAt(curPath.Count- 1);
            n.IsOpen = false;
        }
    }
    
    private class Valve : IEquatable<Valve>
    {
        
        private static Regex _valveRegex = new Regex(@"^Valve (?<name>\w\w) has flow rate=(?<flowRate>\d*); tunnels? leads? to valves? (?<tunnels>.*)$");
        public string Name { get; }
        public int FlowRate { get; }
        public int MinutesOpened { get; private set; }
        public bool IsOpen { get; set; }
        public int TotalPressureRelease => MinutesOpened * FlowRate;
        public List<string> _tunnelDestinationNames = new List<string>();
        public List<Valve> TunnelDestinations { get; set; } = new List<Valve>();

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

        public static bool operator ==(Valve left, Valve right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Valve left, Valve right)
        {
            return !Equals(left, right);
        }
    }
}