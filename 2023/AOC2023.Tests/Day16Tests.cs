using System.Diagnostics;
using System.Text;
using Shouldly;

namespace AOC2023.Tests;

public class Day16Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var grid = get_sample_grid((c, p) => new Tile(c, p));
        var energizedTiles = GetEnergizedTiles(grid, (0, 0, Direction.Right));
        energizedTiles.ShouldBe(46);
    }
    
    protected override void SolvePart1_Actual()
    {
        var grid = get_input_grid((c, p) => new Tile(c, p));
        var energizedTiles = GetEnergizedTiles(grid, (0, 0, Direction.Right));
        energizedTiles.ShouldBe(7996);
    }
    private int GetEnergizedTiles(Tile[,] grid, (int x, int y, Direction laserDirection) start)
    {
        var positions = new Queue<(int x, int y, Direction laserDirection)>();
        var upperBounds = (x: grid.GetLength(0) - 1, y: grid.GetLength(1) - 1);
        positions.Enqueue(start);
        while (positions.Any())
        {
            var p = positions.Dequeue();
            var tile = grid[p.x, p.y];
            //Trace.WriteLine($"({p.x},{p.y}) {p.laserDirection}");
            var nextPositions = tile.NextLaserPositions(p.laserDirection, (0, 0), upperBounds);
            foreach (var np in nextPositions)
                positions.Enqueue(np);
            //Print(grid);
            
        }

        var energizedTiles = grid.Cast<Tile>()
            .Count(x => x.IsEnergized);
        return energizedTiles;
    }
    protected override void SolvePart2_Sample()
    {
        var grid = get_sample_grid((c, p) => new Tile(c, p));
        var energizedTiles = GetMaxEnergizedTiles(grid);
        energizedTiles.ShouldBe(51);
    }

    protected override void SolvePart2_Actual()
    {
        var grid = get_input_grid((c, p) => new Tile(c, p));
        var energizedTiles = GetMaxEnergizedTiles(grid);
        energizedTiles.ShouldBe(8239);
    }

    private int GetMaxEnergizedTiles(Tile[,] grid)
    {
        var upperBounds = (x: grid.GetLength(0) - 1, y: grid.GetLength(1) - 1);
        var maxEnergizedTiles = 0;
        var startPositions = new List<(int x, int y, Direction laserDirection)>();
        startPositions.AddRange(Enumerable.Range(0, upperBounds.x + 1)
            .Select(x => (x, 0, Direction.Down)));
        startPositions.AddRange(Enumerable.Range(0, upperBounds.x + 1)
            .Select(x => (x, upperBounds.y, Direction.Up)));
        startPositions.AddRange(Enumerable.Range(0, upperBounds.y + 1)
            .Select(y => (0, y, Direction.Right)));
        startPositions.AddRange(Enumerable.Range(0, upperBounds.y + 1)
            .Select(y => (upperBounds.x, y, Direction.Left)));

        var tasks = startPositions.Select(x => Task.Run(() =>
        {
            var freshGrid = new Tile[upperBounds.x + 1,upperBounds.y + 1];
            for (int i = 0; i <= upperBounds.x; i++)
                for (int j = 0; j <= upperBounds.y; j++)
                    freshGrid[i, j] = new Tile(grid[i, j].Type, (i, j));
            var energizedTiles = GetEnergizedTiles(freshGrid, x);
            Trace.WriteLine($"({x.x}, {x.y}) {x.laserDirection} => {energizedTiles}");
            return energizedTiles;
        }));
        var maxTiles = Task.WhenAll(tasks).GetAwaiter().GetResult();
        
        return maxTiles.Max();
    }

    
    private void Print(Tile[,] grid)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                sb.Append((grid[x, y].Type != '.' || !grid[x,y].IsEnergized) ? grid[x,y].Type : '#');
            sb.AppendLine();
        }

        Trace.WriteLine(sb.ToString());
    }
    private class Tile
    {
        private char _type;
        public bool IsEnergized = false;
        public char Type => _type;
        public (int x, int y) Position { get; private set; }

        private List<Direction> _incomingLasers = new List<Direction>(); 
        public Tile(char type, (int x, int y) position)
        {
            _type = type;
            Position = position;
        }

        public List<(int x, int y, Direction laserDirection)> NextLaserPositions(Direction laserDirection, 
            (int x, int y) gridLowerBounds,
            (int x, int y) gridUpperBounds)
        {
            var nextPositions = new List<(int x, int y, Direction laserDirection)>();
            IsEnergized = true;
            if (_incomingLasers.Contains(laserDirection))
                return nextPositions; // already processed this tile from this direction
            switch (_type)
            {
                case '.':
                    nextPositions.Add(EmptySpaceNextLocation(laserDirection, Position));
                    break;
                case '\\':
                case '/':
                    nextPositions.Add(MirrorNextLocation(laserDirection, Position, _type));
                    break;
                case '-':
                case '|':
                    nextPositions.AddRange(SplitterNextLocations(laserDirection, Position, _type));
                    break;
            }

            nextPositions.RemoveAll(x => x.x < gridLowerBounds.x || x.x > gridUpperBounds.x ||
                                         x.y < gridLowerBounds.y || x.y > gridUpperBounds.y);
            
            _incomingLasers.Add(laserDirection);
            return nextPositions;
        }

        private (int x, int y, Direction laserDirection) EmptySpaceNextLocation(Direction laserDirection, (int x, int y) current)
        {
            switch (laserDirection)
            {
                case Direction.Up:
                    return (current.x, current.y - 1, laserDirection);
                case Direction.Right:
                    return (current.x + 1, current.y, laserDirection);
                case Direction.Down:
                    return (current.x, current.y + 1, laserDirection);
                case Direction.Left:
                    return (current.x - 1, current.y, laserDirection);
            }
            throw new ArgumentException($"Invalid laser direction: {laserDirection}");
        }
        
        private (int x, int y, Direction laserDirection) MirrorNextLocation(Direction laserDirection, (int x, int y) current, char mirrorType)
        {
            switch (laserDirection)
            {
                case Direction.Up:
                    if (mirrorType == '/')
                        return (current.x + 1, current.y, Direction.Right);
                    // mirrorType == '\'
                    return (current.x - 1, current.y, Direction.Left);
                case Direction.Right:
                    if (mirrorType == '/')
                        return (current.x, current.y - 1, Direction.Up);
                    // mirrorType == '\'
                    return (current.x, current.y + 1, Direction.Down);
                case Direction.Down:
                    if (mirrorType == '/')
                        return (current.x - 1, current.y, Direction.Left);
                    // mirrorType == '\'
                    return (current.x + 1, current.y, Direction.Right);
                case Direction.Left:
                    if (mirrorType == '/')
                        return (current.x, current.y + 1, Direction.Down);
                    // mirrorType == '\'
                    return (current.x, current.y - 1, Direction.Up);
            }
            throw new ArgumentException($"Invalid laser direction: {laserDirection}");
        }

        private List<(int x, int y, Direction laserDirection)> SplitterNextLocations(Direction laserDirection, (int x, int y) current,
            char splitterType)
        {
            var nextPositions = new List<(int x, int y, Direction laserDirection)>();
            switch (laserDirection)
            {
                case Direction.Down:
                case Direction.Up:
                    if (splitterType == '|')
                        nextPositions.Add(EmptySpaceNextLocation(laserDirection, current));
                    else // splitterType == '-'
                    {
                        nextPositions.Add((current.x - 1, current.y, Direction.Left));
                        nextPositions.Add((current.x + 1, current.y, Direction.Right));
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    if (splitterType == '-')
                        nextPositions.Add(EmptySpaceNextLocation(laserDirection, current));
                    else // splitterType == '|'
                    {
                        nextPositions.Add((current.x, current.y - 1, Direction.Up));
                        nextPositions.Add((current.x, current.y + 1, Direction.Down));
                    }
                    break;
            }

            return nextPositions;
        }
    }
    
    private enum Direction { Up, Right, Down, Left }
}