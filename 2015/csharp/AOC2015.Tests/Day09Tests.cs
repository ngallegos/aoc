using AOC.Helpers;

namespace AOC2015.Tests;

/// <summary>
/// https://www.reddit.com/r/adventofcode/comments/3w192e/day_9_solutions/
/// https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
/// </summary>
public class Day09Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var distances = BuildDistances(get_sample(x => new TownPair(x)).ToList());

        // Act
        var shortest = Routing.BestRoute(distances, Math.Min);

        // Assert
        shortest.ShouldBe(605);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var distances = BuildDistances(get_input(x => new TownPair(x)).ToList());

        // Act
        var shortest = Routing.BestRoute(distances, Math.Min);

        // Assert
        shortest.ShouldBe(141);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var distances = BuildDistances(get_sample(x => new TownPair(x)).ToList());

        // Act
        var longest = Routing.BestRoute(distances, Math.Max);

        // Assert
        longest.ShouldBe(982);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var distances = BuildDistances(get_input(x => new TownPair(x)).ToList());

        // Act
        var longest = Routing.BestRoute(distances, Math.Max);

        // Assert
        longest.ShouldBe(736);
    }

    private static int[,] BuildDistances(IReadOnlyList<TownPair> pairs)
    {
        var towns = pairs
            .SelectMany(x => new[] { x.StartTown, x.DestTown })
            .Distinct()
            .Select((town, index) => (town, index))
            .ToDictionary(x => x.town, x => x.index);
        var distances = new int[towns.Count, towns.Count];
        foreach (var pair in pairs)
        {
            var start = towns[pair.StartTown];
            var dest = towns[pair.DestTown];
            distances[start, dest] = distances[dest, start] = pair.Distance;
        }

        return distances;
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
