namespace AOC.Helpers;

public static class GridExtensions
{
    public static List<T> GetAdjacent<T>(this T[][] grid, int x, int y)
    {
        var adjacent = new List<T>();
        var adjacentCoords = new List<(int x, int y)>
        {
            new(x, y + 1),
            new(x + 1, y),
            new(x, y - 1),
            new(x - 1, y),
            new(x + 1, y + 1),
            new(x + 1, y - 1),
            new(x - 1, y + 1),
            new(x - 1, y - 1)
        };

        foreach (var adj in adjacentCoords)
        {
            if (adj.x >= 0 && adj.y >= 0 && adj.y < grid.Length && adj.x < grid[0].Length)
            {
                adjacent.Add(grid[adj.y][adj.x]);
            }
        }
        
        return adjacent;
    }
    
    public static int CountAdjacent<T>(this T[][] grid, int x, int y, Func<T, bool> filter, bool outOfBoundsMatchFilter = true)
    {
        var adjacent = grid.GetAdjacent(x, y);
        var count = adjacent.Count(filter);
        if (outOfBoundsMatchFilter)
        {
            count += 8 - adjacent.Count;
        }
        return count;
    }
    
    public static int CountAdjacent<T>(this List<Coordinate<T>> grid, Coordinate<T> coord, Func<Coordinate<T>, bool> predicate)
    {
        var directions = new List<Coordinate>
        {
            new(0, 1),
            new(1, 0),
            new(0, -1),
            new(-1, 0),
            new(1, 1),
            new(1, -1),
            new(-1, 1),
            new(-1, -1)
        };

        int count = 0;
        foreach (var dir in directions)
        {
            var adjacentCoord = new Coordinate(coord.X + dir.X, coord.Y + dir.Y);
            var adjacent = grid.FirstOrDefault(c => c.X == adjacentCoord.X && c.Y == adjacentCoord.Y);
            if (adjacent != null && predicate(adjacent))
            {
                count++;
            }
        }

        return count;
    }
}