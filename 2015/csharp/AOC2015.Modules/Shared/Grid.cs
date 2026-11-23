using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2015.Modules.Shared;

public class Grid<T>
{
    protected List<List<T>> _grid;
    public List<T> this[int index] => _grid[index];
    protected int Length => _grid.Count;
    protected int _xMinOffset;
    protected int _yMinOffset;

    protected void Initialize(GridLocation upperLeft, GridLocation lowerRight, List<GridLocation<T>> initialValues = null, T defaultValue = default)
    {
        initialValues ??= new List<GridLocation<T>>();
        _xMinOffset = upperLeft.X;
        _yMinOffset = upperLeft.Y;
        var maxY = lowerRight.Y - upperLeft.Y;
        var maxX = lowerRight.X - upperLeft.X;
        _grid = Enumerable.Range(0, maxY + 1)
            .Select(y => Enumerable.Range(0, maxX + 1)
                .Select(x =>
                {
                    var location = initialValues.FirstOrDefault(v => v.X == x + _xMinOffset && v.Y == y + _yMinOffset);
                    if (location != null)
                        return location.Value;
                    return defaultValue;
                }).ToList()).ToList();
    }

    public List<string> Render(Func<T, string> transform = null)
    {
        transform = transform ?? (x => x.ToString()); 
        return _grid.Select(y => string.Join("", y.Select(x => transform(x)))).ToList();
    }
}

public class GridLocation
{
    public GridLocation(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; protected set; }
    public int Y { get; protected set; }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}

public class GridLocation<T> : GridLocation
{
    public GridLocation(int x, int y, T value) : base(x, y)
    {
        Value = value;
    }
    public T Value { get; protected set; }
}