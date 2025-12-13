using AOC.Helpers;

namespace AOC2025.Tests;

public class Day09Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var redTileLocations = get_sample(ParseRedTileLocation).ToArray();
        
        // Act
        var maxArea = FindMaxArea(redTileLocations);
        
        // Assert
        maxArea.ShouldBe(50L);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var redTileLocations = get_input(ParseRedTileLocation).ToArray();
        
        // Act
        var maxArea = FindMaxArea(redTileLocations);
        
        // Assert
        maxArea.ShouldBe(4761736832L);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    (long x, long y) ParseRedTileLocation(string location)
    {
        var locationParts = location.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return (x: int.Parse(locationParts[0]), y: int.Parse(locationParts[1]));
    }
    
    long FindMaxArea((long x, long y)[] redTileLocations)
    {
        var maxArea = 0L;

        for (var i = 0; i < redTileLocations.Length; i++)
        {
            for (var j = i + 1; j < redTileLocations.Length; j++)
            {
                var (x1, y1) = redTileLocations[i];
                var (x2, y2) = redTileLocations[j];

                var area = (Math.Abs(x2 - x1) + 1) * (Math.Abs(y2 - y1) + 1);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }
        }

        return maxArea;
    }
}
