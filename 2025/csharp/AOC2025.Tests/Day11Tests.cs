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
        }).ToArray();
        
        // Act
        var pathCount = CountPaths(devices, "you", "out");
        
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
        }).ToArray();
        
        // Act
        var pathCount = CountPaths(devices, "you", "out");
        
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
        }, partNumber: 2).ToArray();
        
        // Act
        var svrToDac = CountPaths(devices, "svr", "dac");
        var svrToFft = CountPaths(devices, "svr", "fft");
        var dacToFft = CountPaths(devices, "dac", "fft");
        var fftToDac = CountPaths(devices, "fft", "dac");
        var dacToOut = CountPaths(devices, "dac", "out");
        var fftToOut = CountPaths(devices, "fft", "out");
        
        var pathCount = svrToDac * dacToFft * fftToOut 
                        + svrToFft * fftToDac * dacToOut;
        
        // Assert
        pathCount.ShouldBe(2);
    }

    // https://www.reddit.com/r/adventofcode/comments/1pjp1rm/2025_day_11_solutions/
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var devices = get_input(x =>
        {
            var device = x.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Device(device[0], device[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }).ToArray();
        
        // Act
        var svrToDac = CountPaths(devices, "svr", "dac");
        var svrToFft = CountPaths(devices, "svr", "fft");
        var dacToFft = CountPaths(devices, "dac", "fft");
        var fftToDac = CountPaths(devices, "fft", "dac");
        var dacToOut = CountPaths(devices, "dac", "out");
        var fftToOut = CountPaths(devices, "fft", "out");
        
        var pathCount = svrToDac * dacToFft * fftToOut 
                        + svrToFft * fftToDac * dacToOut;
        
        // Assert
        pathCount.ShouldBe(473741288064360);
    }

    long CountPaths(Device[] devices, string start, string end, Dictionary<string, long>? cache = null)
    {
        cache ??= new Dictionary<string, long>();
        
        if (start == end)
        {
            return 1;
        }

        if (cache.TryGetValue(start, out var paths))
        {
            return paths;
        }
        
        var startDevice = devices.FirstOrDefault(x => x.Id == start);
        
        foreach (var output in startDevice?.Outputs ?? [])
        {
            paths += CountPaths(devices, output, end, cache);
        }

        cache[start] = paths;
        
        return paths;
    }
    
    record Device(string Id, string[] Outputs);
}
