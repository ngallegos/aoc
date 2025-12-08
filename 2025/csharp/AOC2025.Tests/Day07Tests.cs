using System.Globalization;

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
        var diagram = get_sample().Select(x => x.Select(c => c.ToString()).ToArray()).ToArray();

        // Act
        PopulateParticlePaths(diagram, 1, Array.IndexOf(diagram[0], "S"));
        var numberOfTimelines = diagram.Last()
            .Select(x => 
                new 
                { 
                    success = int.TryParse(x, out var i),
                    val = i
                })
            .Where(x => x.success)
            .Select(x => x.val).Sum();

        printDiagram(diagram);
        // Assert
        numberOfTimelines.ShouldBe(40);
    }

    [Ignore("Didn't solve - it runs forever so moving on..")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var diagram = get_input().Select(x => x.Select(c => c.ToString()).ToArray()).ToArray();

        // Act
        PopulateParticlePaths(diagram, 1, Array.IndexOf(diagram[0], "S"));
        var numberOfTimelines = diagram.Last()
            .Select(x => 
                new 
                { 
                    success = int.TryParse(x, out var i),
                    val = i
                })
            .Where(x => x.success)
            .Select(x => x.val).Sum();

        // Assert
        numberOfTimelines.ShouldBe(40);
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
    
    private void PopulateParticlePaths(string[][] diagram, int rowIndex, int particleLocation)
    {
        if (rowIndex >= diagram.Length)
        {
            return;
        }
        
        var currentItem = diagram[rowIndex][particleLocation];

        if (currentItem.Equals("."))
        {
            diagram[rowIndex][particleLocation] = "1";
            PopulateParticlePaths(diagram, rowIndex + 1, particleLocation);
            return;
        }

        if (!currentItem.Equals("^"))
        {
            int.TryParse(currentItem, out var count);
            count++;
            diagram[rowIndex][particleLocation] = count.ToString();
            PopulateParticlePaths(diagram, rowIndex + 1, particleLocation);
            return;
        }
        
        PopulateParticlePaths(diagram, rowIndex, particleLocation - 1);
        PopulateParticlePaths(diagram, rowIndex, particleLocation + 1);
    }
    
    private void printDiagram(string[][] diagram)
    {
        foreach (var row in diagram)
        {
            Console.WriteLine(string.Join("", row));
        }
        Console.WriteLine();
    }
}
