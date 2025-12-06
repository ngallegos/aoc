using AOC.Helpers;

namespace AOC2025.Tests;

public class Day04Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var grid = get_sample()
            .Select((line, i) => line.ToArray())
            .ToArray();
        
        // Act
        var accessiblePositions = CountAccessibleRolls(grid);

        // Assert
        accessiblePositions.ShouldBe(13);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var grid = get_input()
            .Select((line, i) => line.ToArray())
            .ToArray();
        
        // Act
        var accessiblePositions = CountAccessibleRolls(grid);

        // Assert
        accessiblePositions.ShouldBe(1587);
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private int CountAccessibleRolls(char[][] grid)
    {
        var accessiblePositions = 0;
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                var value = grid[y][x];
                if (value == '@' && grid.CountAdjacent(x, y, c => c == '.') > 4)
                {
                    accessiblePositions++;
                }
            }
        }
        
        return accessiblePositions;
    }
}