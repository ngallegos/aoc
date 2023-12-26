using System.Text.RegularExpressions;

namespace AOC2023.Tests;

public class Day25Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var s = get_sample(x =>
        {
            var parts = x.Split(":", StringSplitOptions.TrimEntries);
            return new
            {
                vertex = parts[0],
                edges = parts[1].Split(' ', StringSplitOptions.TrimEntries)
                    .Select(d => new List<string> { parts[0], d }.OrderBy(e => e).ToList())
                    .ToList()
            };
        }).ToList();
        var vertices = s.Select(x => x.vertex).ToList();
        var edges = s.SelectMany(x => x.edges).DistinctBy(x => string.Join("", x)).ToList();
        var vCount = vertices.Count;
        while (vCount > 4)
        {
            (vertices, edges) = contract((vertices, edges));
            vCount = vertices.Count;
        }
        
        // not sure what to do with the result yet.

    }

    private static Random _rnd = new Random(DateTimeOffset.UtcNow.Millisecond);
    private (List<string> v, List<List<string>> e) contract((List<string> v, List<List<string>> e) graph)
    {
        var r = _rnd.Next(graph.e.Count);
        var edgeToCut = graph.e[r];
        var newVertex = edgeToCut[0] + edgeToCut[1];
        var newVertexes = graph.v.Where(x => !edgeToCut.Contains(x)).ToList();
        var newEdges = graph.e.Where(x => newVertex != (x[0] + x[1])).ToList();
        newEdges.ForEach(ne =>
        {
            ne.ForEach(nv =>
            {
                if (nv == edgeToCut[0] || nv == edgeToCut[1])
                    nv = newVertex;
            });
        });
        return (newVertexes, newEdges);
    }

    protected override void SolvePart1_Actual()
    {
        throw new NotImplementedException();
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