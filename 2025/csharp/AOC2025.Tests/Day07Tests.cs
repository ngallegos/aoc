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
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new System.NotImplementedException();
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
}
