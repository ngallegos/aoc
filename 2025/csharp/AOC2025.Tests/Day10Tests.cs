using AOC.Helpers;

namespace AOC2025.Tests;

public class Day10Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var machineDefinitions = get_sample(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.ComputeFewestButtonPresses());
        
        // Assert
        shortestPresses.Sum().ShouldBe(7);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var machineDefinitions = get_input(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.ComputeFewestButtonPresses());
        
        // Assert
        shortestPresses.Sum().ShouldBe(520);
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
            LightValue = Convert.ToInt64(LightDiagram.Replace('#', '1').Replace('.', '0'), 2);
            WiringSchematics = parts[1..^1].Select(x => x.Trim('(', ')').Split(',').Select(int.Parse).ToArray()).ToArray();
            WiringSchematicsValues = new long[WiringSchematics.Length];
            for (var i = 0; i < WiringSchematics.Length; i++)
            {
                var emptyDiagram = new int[LightDiagram.Length];
                foreach (var index in WiringSchematics[i])
                {
                    emptyDiagram[index] = 1;
                }
                WiringSchematicsValues[i] = Convert.ToInt64(string.Join("",emptyDiagram.Select(x => x.ToString())), 2);
            }
            JoltageRequirements = parts[^1].Trim('{', '}').Split(',').Select(int.Parse).ToArray();
        }

        public int ComputeFewestButtonPresses()
        {
            var path = Search<long>.BFS(0L, 
                x => WiringSchematicsValues.Select(s => x ^ s).ToList(), 
                x => x == LightValue);

            return (path?.Count ?? 0) - 1;
        }

        string Definition { get; }
        long LightValue { get; }
        string LightDiagram { get; }
        int[][] WiringSchematics { get; }
        long[] WiringSchematicsValues { get; }
        int[] JoltageRequirements { get; }
    }
}
