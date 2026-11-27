using System.Numerics;

namespace AOC.Helpers;

public static class Routing
{
    /// <summary>
    /// Best total distance of a route visiting every node exactly once, starting and ending anywhere.
    /// Time and memory grow with 2^n, so it is practical up to about 24 nodes.
    /// </summary>
    /// <param name="distances">Square matrix where [from, to] is the distance between two nodes.</param>
    /// <param name="best">Picks the preferred of two totals, e.g. Math.Min or Math.Max.</param>
    public static int BestRoute(int[,] distances, Func<int, int, int> best)
    {
        // Held-Karp: routes[visited * n + last] is the best route over the visited bitmask ending at last
        var n = distances.GetLength(0);
        var all = (1 << n) - 1;
        var routes = new int?[(1 << n) * n];
        for (var node = 0; node < n; node++)
        {
            routes[(1 << node) * n + node] = 0;
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
}
