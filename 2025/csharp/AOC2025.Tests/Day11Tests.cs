namespace AOC2025.Tests;

public class Day11Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var devices = get_sample(x =>
        {
            var device = x.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Device(device[0], device[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }).ToList();
        
        // Act
        var pathCount = FindPaths("you", "out", devices.ToArray());
        
        // Assert
        pathCount.ShouldBe(5);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var devices = get_input(x =>
        {
            var device = x.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Device(device[0], device[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }).ToList();
        
        // Act
        var pathCount = FindPaths("you", "out", devices.ToArray());
        
        // Assert
        pathCount.ShouldBe(696L);
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

    long FindPaths(string start, string end, Device[] devices, long count = 0)
    {
        if (start == end)
            return 1;
        
        var startDevice = devices.First(x => x.Id == start);
        foreach (var output in startDevice.Outputs)
        {
            count += FindPaths(output, end, devices);
        }

        return count;
    }
    
    record Device(string Id, string[] Outputs);
}
