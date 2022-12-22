using System.Linq;

namespace AOC2015.Modules;

/// <summary>
/// https://www.reddit.com/r/adventofcode/comments/3w192e/day_9_solutions/
/// https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
/// </summary>
public class Day09 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var pairs = get_sample()
            .Select(x => new TownPair(x))
            .ToList();
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class TownPair
    {
        public string StartTown { get; private set; }
        public string DestTown { get; private set; }
        public int Distance { get; private set; }

        public TownPair(string input)
        {
            var parts = input.Split(" = ");
            var towns = parts[0].Split(" to ");
            StartTown = towns[0];
            DestTown = towns[1];
            Distance = int.Parse(parts[1]);
        }
    }
}