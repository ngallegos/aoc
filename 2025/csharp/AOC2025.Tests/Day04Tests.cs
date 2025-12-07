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
        var accessiblePositions = GetAccessibleRolls(grid).Count;

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
        var accessiblePositions = GetAccessibleRolls(grid).Count;

        // Assert
        accessiblePositions.ShouldBe(1587);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var grid = get_sample()
            .Select((line, i) => line.ToArray())
            .ToArray();
        
        // Act
        var accessiblePositions = RemoveAccessibleRolls(grid);

        // Assert
        accessiblePositions.ShouldBe(43);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var grid = get_input()
            .Select((line, i) => line.ToArray())
            .ToArray();
        
        // Act
        var accessiblePositions = RemoveAccessibleRolls(grid);

        // Assert
        accessiblePositions.ShouldBe(8946);
    }

    private List<(int, int)> GetAccessibleRolls(char[][] grid)
    {
        var accessibleRolls = new List<(int, int)>();
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                var value = grid[y][x];
                if (value == '@' && grid.CountAdjacent(x, y, c => c == '.') > 4)
                {
                    accessibleRolls.Add((x, y));
                }
            }
        }
        
        return accessibleRolls;
    }

    private int RemoveAccessibleRolls(char[][] grid)
    {
        var accessibleRolls = GetAccessibleRolls(grid);

        if (accessibleRolls.Count == 0)
        {
            return 0;
        }
        
        foreach (var (x, y) in accessibleRolls)
        {
            grid[y][x] = '.';
        }

        return accessibleRolls.Count + RemoveAccessibleRolls(grid);
    }
}