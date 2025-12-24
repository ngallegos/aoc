
namespace AOC2025.Tests;

public class Day07Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var diagram = get_sample().ToArray();

        // Act
        var numberOfBeamSplits = GetNumberOfBeamSplits(diagram);

        // Assert
        numberOfBeamSplits.ShouldBe(21);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var diagram = get_input().ToArray();

        // Act
        var numberOfBeamSplits = GetNumberOfBeamSplits(diagram);

        // Assert
        numberOfBeamSplits.ShouldBe(1628);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var diagram = get_sample().ToArray();

        // Act
        var paths = CountPaths(diagram, 1, diagram[0].IndexOf('S'));

        // Assert
        paths.ShouldBe(40L);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var diagram = get_input().ToArray();

        // Act
        var paths = CountPaths(diagram, 1, diagram[0].IndexOf('S'));

        // Assert
        paths.ShouldBe(27055852018812L);
    }
    
    private int GetNumberOfBeamSplits(string[] diagram)
    {
        var currentBeamLocations = new List<int> { diagram.First().IndexOf('S') };
        var numberOfBeamSplits = 0;
        foreach (var row in diagram[1..])
        {
            var splitterLocationInBeamPath = Enumerable.Range(0, row.Length)
                .Where(i => row[i] == '^' && currentBeamLocations.Contains(i))
                .ToList();
            
            numberOfBeamSplits += splitterLocationInBeamPath.Count;
            
            currentBeamLocations.RemoveAll(x => splitterLocationInBeamPath.Contains(x));
            foreach (var splitterLocation in splitterLocationInBeamPath)
            {
                currentBeamLocations.Add(splitterLocation - 1);
                currentBeamLocations.Add(splitterLocation + 1);
            }
            currentBeamLocations.RemoveAll(x => x < 0 || x >= row.Length);
            currentBeamLocations = currentBeamLocations.Distinct().ToList();
        }
        
        return numberOfBeamSplits;
    }

    private long CountPaths(string[] diagram, int index, int location, Dictionary<(int index, int location), long>? cache = null)
    {
        cache ??= new Dictionary<(int index, int location), long>();

        if (index >= diagram.Length)
            return 1;

        if (location >= diagram[index].Length || location < 0)
            return 0;
        
        if (cache.ContainsKey((index, location)))
            return cache[(index, location)];

        long paths;
        
        if (diagram[index][location] == '^')
            paths = CountPaths(diagram, index + 1, location + 1, cache) + CountPaths(diagram, index + 1, location - 1, cache);
        else
            paths = CountPaths(diagram, index + 1, location, cache);
        
        cache.Add((index, location), paths);
        return paths;
    }
}
