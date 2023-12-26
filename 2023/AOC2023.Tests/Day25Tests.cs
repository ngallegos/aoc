using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day25Tests : TestBase
{
    
    //https://aoc.csokavar.hu/?day=25
    protected override void SolvePart1_Sample()
    {
        var input = get_sample().ToList();
        // call Karger's algorithm in a loop until it finds a cut with 3 edges:
        var (cutSize, c1, c2) = FindCut(input);
        while (cutSize != 3) {
            (cutSize, c1, c2) = FindCut(input);
        }
        var groupProduct = c1 * c2; 
        groupProduct.ShouldBe(54);
    }

    private static Random _rnd = new Random(DateTimeOffset.UtcNow.Millisecond);

    protected override void SolvePart1_Actual()
    {
        throw new NotImplementedException();
    }
    // https://en.wikipedia.org/wiki/Karger%27s_algorithm
    // The Karger algorithm returns the size of one 'cut' of the graph. 
    // It's a randomized algorithm that is 'likely' to find the minimal cut 
    // in a reasonable time. The algorithm is extended to also return the sizes 
    // of the two components separated by the cut.
    (int size, int c1, int c2) FindCut(List<string> input) {
 
        // super inefficient version.
 
        var graph = Parse(input);
        var componentSize = graph.Keys.ToDictionary(k => k, _ => 1);
 
        while (graph.Count > 2) {
            // choose a random edge u-v to contract.
            var u = graph.Keys.ElementAt(_rnd.Next(graph.Count));
            var v = graph[u][_rnd.Next(graph[u].Count)];
 
            // Merge nodes u and v by removing 'v' from the graph and rebinding 
            // the edges of 'v' so that they start from 'u' now. 
            // There are no multiple edges between two nodes in the original
            // graph, but this algorithm will introduce some.
 
            // rebind
            foreach (var neighbour in graph[v].ToArray())
            {
                if (graph.TryGetValue(neighbour, out var value))
                {
                    value.Remove(v);
                    value.Add(u);
                }
            }
 
            // 'u' inherits 'v'-s edges
            graph[u] = graph[u].Concat(graph[v]).Distinct().ToList();

            // update component size 
            componentSize[u] = componentSize[u] + componentSize[v];
          
            // 'v' can be removed now
            graph.Remove(v);
            componentSize.Remove(v);
        }
 
        // at the end we have just two nodes with some edges between them,
        // the number of those edges equals to the size of the cut
        var nodeA = graph.Keys.First();
        var nodeB = graph.Keys.Last();
        return (graph[nodeA].Count, componentSize[nodeA], componentSize[nodeB]);
    }
 
    // Returns an adjacency list representation of the input. Edges are recorded 
    // both ways, unlike in the input which contains them in one direction only.
    Dictionary<string, List<string>> Parse(List<string> input) {
        Dictionary<string, List<string>> res = new();
 
        var registerEdge = (string u, string v) => {
            if (!res.ContainsKey(u)) {
                res[u] = new();
            }
            res[u].Add(v);
        };
 
        foreach (var line in input) {
            var parts = line.Split(": ");
            var u = parts[0];
            var nodes = parts[1].Split(' ');
            foreach (var v in nodes) {
                registerEdge(u, v);
                registerEdge(v, u);
            }
        }
        return res;
    }
    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }
}