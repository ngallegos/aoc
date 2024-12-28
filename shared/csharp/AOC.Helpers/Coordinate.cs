namespace AOC.Helpers;

public class Coordinate(int x, int y)
{
    public int X { get; protected set; } = x;
    public int Y { get; protected set; } = y;

    public override string ToString()
    {
        return $"({X},{Y})";
    }

    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinate operator +(Coordinate a, (int x, int y) b) => new(a.X + b.x, a.Y + b.y);
}

public class Coordinate<T> : Coordinate
{
    public Coordinate(int x, int y, T value) : base(x, y)
    {
        Value = value;
    }
    public T Value { get; protected set; }
}