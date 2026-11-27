using System.Numerics;

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
        var shortest = BestRoute(distances, Math.Min);

        // Assert
        shortest.ShouldBe(605);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var distances = BuildDistances(get_input(x => new TownPair(x)).ToList());

        // Act
        var shortest = BestRoute(distances, Math.Min);

        // Assert
        shortest.ShouldBe(141);
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

    // Held-Karp: routes[visited * n + last] is the best route over the visited bitmask ending at last
    private static int BestRoute(int[,] distances, Func<int, int, int> best)
    {
        var n = distances.GetLength(0);
        var all = (1 << n) - 1;
        var routes = new int?[(1 << n) * n];
        for (var town = 0; town < n; town++)
        {
            routes[(1 << town) * n + town] = 0;
        }

        for (var visited = 1; visited <= all; visited++)
        {
            for (var ends = visited; ends != 0; ends &= ends - 1)
            {
                var last = BitOperations.TrailingZeroCount(ends);
                var route = routes[visited * n + last]!.Value;
                for (var unvisited = all & ~visited; unvisited != 0; unvisited &= unvisited - 1)
                {
                    var next = BitOperations.TrailingZeroCount(unvisited);
                    var index = (visited | 1 << next) * n + next;
                    var candidate = route + distances[last, next];
                    routes[index] = routes[index] is { } current ? best(current, candidate) : candidate;
                }
            }
        }

        return Enumerable.Range(0, n).Select(last => routes[all * n + last]!.Value).Aggregate(best);
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
