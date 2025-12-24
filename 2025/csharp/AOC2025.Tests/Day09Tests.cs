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
        var redTileLocations = get_sample(ParseRedTileLocation).ToArray();
        var redTileShape = new Shape(redTileLocations);
        var rectangles = GetAllRectangles(redTileLocations)
            .OrderByDescending(x => x.Area)
            .ToList();
        
        // Act
        var maxCoveredArea = 0L;
        foreach (var rectangle in rectangles)
        {
            if (redTileShape.Contains(rectangle))
            {
                maxCoveredArea = rectangle.Area;
                break;
            }
        }
        
        // Assert
        maxCoveredArea.ShouldBe(24L);
    }

    [Ignore("Incorrect right now - too high at 4598541075L")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var redTileLocations = get_input(ParseRedTileLocation).ToArray();
        var redTileShape = new Shape(redTileLocations);
        var rectangles = GetAllRectangles(redTileLocations)
            .OrderByDescending(x => x.Area)
            .ToList();
        
        // Act
        var maxCoveredArea = 0L;
        foreach (var rectangle in rectangles)
        {
            if (redTileShape.Contains(rectangle))
            {
                maxCoveredArea = rectangle.Area;
                break;
            }
        }
        
        // Assert
        maxCoveredArea.ShouldBe(24L);
    }

    (long x, long y) ParseRedTileLocation(string location)
    {
        var locationParts = location.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return (x: int.Parse(locationParts[0]), y: int.Parse(locationParts[1]));
    }

    class Shape
    {
        private List<(long x, long y)> Points { get; init; }
        public Shape((long x, long y)[] redTiles)
        {
            Points = new List<(long, long)>();
            Points.AddRange(redTiles);
        }
        
        public (long x, long y) this[int index] => index < 0 ? Points[^1] : Points[index % Points.Count];

        public bool Contains(Rectangle rectangle)
        {
            var corners = new List<(long, long)>
            {
                (rectangle.DiagonalCorner1.x, rectangle.DiagonalCorner1.y),
                (rectangle.DiagonalCorner1.x, rectangle.DiagonalCorner2.y),
                (rectangle.DiagonalCorner2.x, rectangle.DiagonalCorner1.y),
                (rectangle.DiagonalCorner2.x, rectangle.DiagonalCorner2.y)
            };
            var containedRedTiles = Points
                .Where(p => !corners.Contains((p.x, p.y)))
                .Where(p => rectangle.Contains(p.x, p.y))
                .ToArray();

            if (containedRedTiles.Length == 0)
                return true;

            foreach (var redTile in containedRedTiles)
            {
                var index = Points.IndexOf(redTile);
                var previous = this[index - 1];
                var next = this[index + 1];

                if (rectangle.Contains(previous.x, previous.y) || rectangle.Contains(next.x, next.y))
                    return false;
                // todo - find if its neighbors go outside the rectangle
            }
            
            return true;
        }
    }
    
    List<Rectangle> GetAllRectangles((long x, long y)[] redTileLocations)
    {
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < redTileLocations.Length; i++)
        {
            for (var j = i + 1; j < redTileLocations.Length; j++)
            {
                rectangles.Add(new Rectangle(redTileLocations[i], redTileLocations[j]));
            }
        }

        return rectangles;
    }
    
    long FindMaxArea((long x, long y)[] redTileLocations)
    {
        var rectangles = GetAllRectangles(redTileLocations);
        return FindMaxArea(rectangles);
    }
    
    long FindMaxArea(List<Rectangle> rectangles)
    {
        var maxArea = 0L;

        foreach (var rectangle in rectangles)
        {
            if (rectangle.Area > maxArea)
            {
                maxArea = rectangle.Area;
            }
        }

        return maxArea;
    }
    
    HashSet<(long x, long y)> GetCoveredPositions(List<Rectangle> rectangles)
    {
        var coveredPositions = new HashSet<(long x, long y)>();

        foreach (var rectangle in rectangles)
        {
            coveredPositions = coveredPositions.Concat(rectangle.CoveredPositions).ToHashSet();
            for (var x = rectangle.MinX; x <= rectangle.MaxX; x++)
            {
                for (var y = rectangle.MinY; y <= rectangle.MaxY; y++)
                {
                    coveredPositions.Add((x, y));
                }
            }
        }

        return coveredPositions;
    }

    record Rectangle((long x, long y) DiagonalCorner1, (long x, long y) DiagonalCorner2)
    {
        public (long x, long y) DiagonalCorner1 { get; init; } = DiagonalCorner1;
        public (long x, long y) DiagonalCorner2 { get; init; } = DiagonalCorner2;
        public long MinX { get; init; } = Math.Min(DiagonalCorner1.x, DiagonalCorner2.x);
        public long MaxX { get; init; } = Math.Max(DiagonalCorner1.x, DiagonalCorner2.x);
        public long MinY { get; init; } = Math.Min(DiagonalCorner1.y, DiagonalCorner2.y);
        public long MaxY { get; init; } = Math.Max(DiagonalCorner1.y, DiagonalCorner2.y);
        public long Width { get; } = Math.Abs(DiagonalCorner2.x - DiagonalCorner1.x) + 1;
        public long Height { get; } = Math.Abs(DiagonalCorner2.y - DiagonalCorner1.y) + 1;
        public long Area => Width * Height;

        public bool Contains(long x, long y)
        {
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }


        private HashSet<(long x, long y)> _coveredPositions = new();
        public HashSet<(long x, long y)> CoveredPositions {
            get
            {
                if (_coveredPositions.Count == 0)
                {

                    for (var x = MinX; x <= MaxX; x++)
                    {
                        for (var y = MinY; y <= MaxY; y++)
                        {
                            _coveredPositions.Add((x, y));
                        }
                    }
                }

                return _coveredPositions;
            }
        }

        public bool AllPositionsCovered(HashSet<(long x, long y)> coveredPositions)
        {
            return CoveredPositions.All(coveredPositions.Contains);
        }
    }
}
