using System;
using System.Collections.Generic;
using System.Linq;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day23 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private static (int x, int y)[] _neighbors = new[]
    {
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (-1, 0),
        (-1, -1),
        (-1, 0),
        (-1, 1),
    };

    private class Scan
    {
        private bool[,] _grid;

        public Scan(List<string> lines)
        {
            _grid = new bool[lines[0].Length, lines.Count];
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    _grid[x, y] = lines[y][x] == '#';
                }
            }
        }
        
        private bool[] Column(int column)
        {
            return Enumerable.Range(0, _grid.GetLength(0))
                .Select(x => _grid[x, column])
                .ToArray();
        }
        
        private bool[] Row(int row)
        {
            return Enumerable.Range(0, _grid.GetLength(1))
                .Select(x => _grid[row, x])
                .ToArray();
        }

        public void Resize(int newWidth, int newHeight)
        {
            _grid = _grid.ResizeArray(newWidth, newHeight, 1, 1);
        }
        
    }

    
    
    private class Elf
    {
        public GridLocation Location { get; private set; }
        public GridLocation Proposed { get; private set; }

        public Elf(GridLocation initialLocation)
        {
            Location = initialLocation;
        }

        // public GridLocation ConsiderNewLocation(List<Elf> elves)
        // {
        //     var adjacent = _neighbors.Select(l => this.Location + l);
        //     //foreach (var elf in elves)
        //         
        // }
    }
}

public static class Extensions
{
    public static T[,] ResizeArray<T>(this T[,] original, int newWidth, int newHeight, int offsetX = 0, int offsetY = 0)
    {
        T[,] newArray = new T[newWidth, newHeight];
        int width = original.GetLength(0);
        int height = original.GetLength(1);
        for (int x = 0; x < width; x++) {
            Array.Copy(original, x * height, newArray, (x + offsetX) * newHeight + offsetY, height);
        }

        return newArray;
    }
}