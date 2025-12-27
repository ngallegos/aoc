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
        var maxCoveredArea = 0.0;
        foreach (var rectangle in rectangles)
        {
            if (redTileShape.IsInShape(rectangle))
            {
                maxCoveredArea = rectangle.Area;
                break;
            }
        }
        
        // Assert
        maxCoveredArea.ShouldBe(24L);
    }

    //[Ignore("Incorrect right now - too high at 4598541075L")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var redTileLocations = get_input(ParseRedTileLocation).ToArray();
        var redTileShape = new Shape(redTileLocations);
        var rectangles = GetAllRectangles(redTileLocations)
            .OrderByDescending(x => x.Area)
            .ToList();
        
        // Act
        var maxCoveredArea = 0.0;
        foreach (var rectangle in rectangles)
        {
            if (redTileShape.IsInShape(rectangle))
            {
                maxCoveredArea = rectangle.Area;
                break;
            }
        }
        
        // Assert
        var invalidAnswers =  new Dictionary<double, string>
        {
            { 4598541075, "too high" },
            { 702645, "too low" },
            { 1417, "too low" }
        };

        if (invalidAnswers.TryGetValue(maxCoveredArea, out var answer))
            throw new Exception($"{maxCoveredArea} is {answer}");
        
        maxCoveredArea.ShouldBe(24L);
    }

    Point ParseRedTileLocation(string location)
    {
        var locationParts = location.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return new (int.Parse(locationParts[0]), int.Parse(locationParts[1]));
    }

    class Shape
    {
        List<Point> Points { get; init; }
        int Length => Points.Count;

        public Shape(Point[] redTiles)
        {
            Points = new List<Point>();
            Points.AddRange(redTiles);
        }
        
        public Point this[int index] => index < 0 ? Points[^1] : Points[index % Points.Count];
        
        bool IsInShape(Point point)
        {
            var n = Points.Count;
            bool inside = false;
        
            var p1 = this[0];
            for (int i = 1; i <= n; i++)
            {
                var p2 = this[i];
            
                if (point.Y >= Math.Min(p1.Y, p2.Y))
                {
                    if (point.Y <= Math.Max(p1.Y, p2.Y))
                    {
                        if (point.X <= Math.Max(p1.X, p2.X))
                        {
                            if (p1.Y != p2.Y)
                            {
                                double xInters = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                                if (p1.X == p2.X || point.X <= xInters)
                                {
                                    inside = !inside;
                                }
                            }
                        }
                    }
                }
            
                p1 = p2;
            }
        
            return inside;
        }

        
        public bool IsInShape(Rectangle rectangle)
        {
            // Check if corners are in the shape
            if (!rectangle.Corners.All(IsInShape)) return false;
            
            // Check if any rectangle edge intersects with shape boundary
            for (int i = 0; i < this.Length; i++)
            {
                var boundaryEdge = new Segment(this[i], this[i + 1]);
            
                foreach (var rectEdge in rectangle.Edges)
                {
                    if (rectEdge.Intersects(boundaryEdge, out var intersection))
                    {
                        if (intersection != null)
                        {
                            if (rectangle.Contains(intersection, false)
                                && !Points.Contains(intersection)
                                && !rectEdge.Contains(intersection, false))
                                return false;
                            // if (!Points.Contains(intersection))
                            //     return false;
                            // if (!rectEdge.Contains(intersection, false))
                            //     return false;
                        }
                    }
                }
            }
        
            return true;
        }
    }
    
    List<Rectangle> GetAllRectangles(Point[] redTileLocations)
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
    
    double FindMaxArea(Point[] redTileLocations)
    {
        var rectangles = GetAllRectangles(redTileLocations);
        return FindMaxArea(rectangles);
    }
    
    double FindMaxArea(List<Rectangle> rectangles)
    {
        var maxArea = 0.0;

        foreach (var rectangle in rectangles)
        {
            if (rectangle.Area > maxArea)
            {
                maxArea = rectangle.Area;
            }
        }
        
        return maxArea;
    }

    record Point(double X, double Y)
    {
        public virtual bool Equals(Point? other)
        {
            if (other == null)
                return false;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    record Line(double A, double B, double C)
    {
        public Point? GetIntersectionPoint(Line other)
        {
            var delta = this.A * other.B - other.A * this.B;

            if (delta == 0) // parallel
                return null;

            var x = (other.B * this.C - this.B * other.C) / delta;
            var y = (this.A * other.C - other.A * this.C) / delta;
            return new Point(x, y);
        
        }
    }

    record Segment(Point P1, Point P2)
    {
        public Segment((double X, double Y) p1, (double X, double Y) p2) : 
            this(new Point(p1.X, p1.Y), new Point(p2.X, p2.Y))
        {
        }

        // For Ax + By = C definition of a line
        Line GetLineDefinition()
        {
            var a = P1.Y - P2.Y;
            var b = P2.X - P1.X;
            
            var c= -((P1.X * P2.Y) - (P2.X * P1.Y)); 
            
            return new Line(a, b, c);
        }

        Point? GetIntersectionPoint(Segment other)
        {
            var line1 = this.GetLineDefinition();
            var line2  = other.GetLineDefinition();
            
            return line1.GetIntersectionPoint(line2);
        }

        public bool Intersects(Segment boundaryEdge, out Point? intersection)
        {
            intersection = GetIntersectionPoint(boundaryEdge);
            if (intersection == null)
                return false;

            return true;
        }

        public bool Contains(Point? p, bool includeEndpoints = true)
        {
            if (p == null)
                return false;

            if (!includeEndpoints)
            {
                if (P1.Equals(p) || P2.Equals(p))
                    return false;
            }
            
            if (P1.X == P2.X && P1.X == p.X)
                return true;

            if (P1.Y == P2.Y && P1.Y == p.Y)
                return true;
            
            return false;
        }
    }

    record Rectangle(Point DiagonalCorner1, Point DiagonalCorner2)
    {
        public double MinX { get; init; } = Math.Min(DiagonalCorner1.X, DiagonalCorner2.X);
        public double MaxX { get; init; } = Math.Max(DiagonalCorner1.X, DiagonalCorner2.X);
        public double MinY { get; init; } = Math.Min(DiagonalCorner1.Y, DiagonalCorner2.Y);
        public double MaxY { get; init; } = Math.Max(DiagonalCorner1.Y, DiagonalCorner2.Y);
        public double Width { get; } = Math.Abs(DiagonalCorner2.X - DiagonalCorner1.X) + 1;
        public double Height { get; } = Math.Abs(DiagonalCorner2.Y - DiagonalCorner1.Y) + 1;
        public double Area => Width * Height;
        
        public Point[] Corners { get; } =
        [
            new (DiagonalCorner1.X, DiagonalCorner1.Y),
            new (DiagonalCorner1.X, DiagonalCorner2.Y),
            new (DiagonalCorner2.X, DiagonalCorner1.Y),
            new (DiagonalCorner2.X, DiagonalCorner2.Y)
        ];

        public Segment[] Edges =>
        [
            new (Corners[0], Corners[1]),
            new (Corners[1], Corners[2]),
            new (Corners[2], Corners[3]),
            new (Corners[3], Corners[0]),
        ];

        public bool Contains(Point? p, bool includeBorder = true)
        {
            if (p == null)
                return false;
            
            if (includeBorder)
                return p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;
            
            return p.X > MinX && p.X < MaxX && p.Y > MinY && p.Y < MaxY;
        }
    }
}
