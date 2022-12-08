using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace AOC2022.Modules;

public class Day08 : DayBase
{
    public override dynamic Part1()
    {
        var treeHeightGrid = new TreeHeightGrid(get_input(s =>
        {
            return s.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
        }).ToArray());

        return new { visibleTrees = treeHeightGrid.NumberOfTreesVisibleFromPerimeter() };
    }

    public override dynamic Part2()
    {
        return "Not implemented";
    }

    public class TreeHeightGrid
    {
        public readonly int[][] HeightGrid;
        public TreeHeightGrid(int[][] input)
        {
            HeightGrid = input;
        }

        public bool TreeIsVisibleFromPerimeter(int x, int y)
        {
            var treeHeight = HeightGrid[x][y];
            if (x == 0
                || y == 0
                || x == HeightGrid[x].Length - 1
                || y == HeightGrid.Length - 1)
                return true;
            var eastView = HeightGrid[y].Take(x).All(t => t < treeHeight);
            var westView = HeightGrid[y].Skip(x + 1).All(t => t < treeHeight);
            var northView = HeightGrid.Take(y).Select(c => c[x]).All(t => t < treeHeight);
            var southView = HeightGrid.Skip(y+1).Select(c => c[x]).All(t => t < treeHeight);

            return eastView || westView || northView || southView;
        }

        public int NumberOfTreesVisibleFromPerimeter()
        {
            var visibleTrees = 0;
            for (int i = 0; i < HeightGrid[0].Length; i++)
            for (int j = 0; j < HeightGrid.Length; j++)
                visibleTrees += TreeIsVisibleFromPerimeter(i, j) ? 1 : 0;
            return visibleTrees;
        }
    }
}