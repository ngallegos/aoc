namespace AOC2015.Tests;

/// <summary>
/// https://www.reddit.com/r/adventofcode/comments/3w192e/day_9_solutions/
/// https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
/// </summary>
public class Day09Tests : TestBase
{
    [Ignore("Not attempted yet")]
    protected override void SolvePart1_Sample()
    {
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart1_Actual()
    {
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Sample()
    {
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Actual()
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
