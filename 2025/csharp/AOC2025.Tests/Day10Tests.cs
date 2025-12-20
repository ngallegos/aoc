namespace AOC2025.Tests;

public class Day10Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var machineDefinitions = get_sample(x => new Machine(x)).ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart1_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    class Machine
    {
        public Machine(string definition)
        {
            var parts = definition.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            LightDiagram = parts[0].Trim('[', ']');
            WiringSchematics = parts[1..^1].Select(x => x.Trim('(', ')').Split(',').Select(int.Parse).ToArray()).ToArray();
            JoltageRequirements = parts[^1].Trim('{', '}').Split(',').Select(int.Parse).ToArray();
        }

        string LightDiagram { get; }
        int[][] WiringSchematics { get; }
        int[] JoltageRequirements { get; }
    }
}
