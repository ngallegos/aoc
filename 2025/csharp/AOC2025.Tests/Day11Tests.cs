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
        var pathCount = FindPaths("you", "out", devices.ToArray()).Count;
        
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
        var pathCount = FindPaths("you", "out", devices.ToArray()).Count;
        
        // Assert
        pathCount.ShouldBe(696);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var devices = get_sample(x =>
        {
            var device = x.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Device(device[0], device[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }, partNumber: 2).ToList();
        
        // Act
        var pathCount = FindPaths("svr", "out", devices.ToArray())
            .Count(x =>
            {
                var pathSegments = x.Split(',');
                return pathSegments.Contains("dac") && pathSegments.Contains("fft");
            });
        
        // Assert
        pathCount.ShouldBe(2);
    }

    [Ignore("Running way too long...")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var devices = get_input(x =>
        {
            var device = x.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Device(device[0], device[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }).ToList();
        
        // Act
        var paths = FindPaths("svr", "out", devices.ToArray());
        var pathCount = paths
            .Count(x =>
            {
                var pathSegments = x.Split(',');
                return pathSegments.Contains("dac") && pathSegments.Contains("fft");
            });
        
        // Assert
        pathCount.ShouldBe(2);
    }

    List<string> FindPaths(string start, string end, Device[] devices, string currentPath = "", List<string> paths = null)
    {
        paths ??= new List<string>();
        if (start == end)
        {
            paths.Add(currentPath);
            return paths;
        }

        var startDevice = devices.FirstOrDefault(x => x.Id == start);
        foreach (var output in startDevice.Outputs)
        {
            paths = FindPaths(output, end, devices, currentPath + "," + output, paths);
        }

        return paths;
    }
    
    record Device(string Id, string[] Outputs);
}
