using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day08 : DayBase
{
    public override bool Ignore => true;

    private int[][] _sample = new[]
    {
        new[] { 3, 0, 3, 7, 3 },
        new[] { 2, 5, 5, 1, 2 },
        new[] { 6, 5, 3, 3, 2 },
        new[] { 3, 3, 5, 4, 9 },
        new[] { 3, 5, 3, 9, 0 }
    };
    
    public override dynamic Part1()
    {
        var treeHeightGrid = new TreeHeightGrid(get_input(s =>
        {
            return s.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
        }, "Part1").ToArray());
        return new { visibleTrees = treeHeightGrid.NumberOfTreesVisibleFromPerimeter() };
    }

    public override dynamic Part2()
    {
        var treeHeightGrid = new TreeHeightGrid(get_input(s =>
        {
            return s.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
        }, "Part1").ToArray());
        var bestTree = treeHeightGrid.HighestScenicScoringTree();
        return new { bestTree };
    }

    public class TreeHeightGrid
    {
        private readonly int[][] _heightGrid;
        private List<Tree> _trees;
        public TreeHeightGrid(int[][] input)
        {
            _heightGrid = input;
            _trees = new List<Tree>();
            for (int i = 0; i < _heightGrid[0].Length; i++)
            for (int j = 0; j < _heightGrid.Length; j++)
                _trees.Add(new Tree(i,j, this));
        }

        public int[] this[int index]
        {
            get { return _heightGrid[index]; }
        }

        public IEnumerable<int[]> Take(int numToTake) => _heightGrid.Take(numToTake);
        public IEnumerable<int[]> Skip(int numToSkip) => _heightGrid.Skip(numToSkip);

        public int Length => _heightGrid.Length;

        public int NumberOfTreesVisibleFromPerimeter()
        {
            return _trees.Count(x => x.VisibleFromPerimeter);
        }

        public Tree HighestScenicScoringTree()
        {
            return _trees.MaxBy(x => x.ScenicScore);
        }
    }

    public class Tree
    {
        public Tree(int x, int y, TreeHeightGrid grid)
        {
            Location = (x, y);
            Height = grid[y][x];
            NorthernView = grid.Take(y).Select(c => c[x]).Reverse().ToList();
            SouthernView = grid.Skip(y + 1).Select(c => c[x]).ToList();
            EasternView = grid[y].Skip(x + 1).ToList();
            WesternView = grid[y].Take(x).Reverse().ToList();
            SetVisibilityFromPerimeter(grid);
            VisibilityNorth = CountTreesInView(NorthernView);
            VisibilitySouth = CountTreesInView(SouthernView);
            VisibilityEast = CountTreesInView(EasternView);
            VisibilityWest = CountTreesInView(WesternView);
        }
        
        private (int x, int y) Location { get; set; }
        private int Height { get; set; }
        
        private List<int> NorthernView { get; set; }
        private List<int> SouthernView { get; set; }
        private List<int> EasternView { get; set; }
        private List<int> WesternView { get; set; }
        
        private int VisibilityNorth { get; set; }
        private int VisibilitySouth { get; set; }
        private int VisibilityEast { get; set; }
        private int VisibilityWest { get; set; }

        public int ScenicScore => VisibilityNorth * VisibilitySouth * VisibilityEast * VisibilityWest;
        public bool VisibleFromPerimeter { get; private set; }

        private int CountTreesInView(IEnumerable<int> view)
        {
            var visibleTrees = 0;
            foreach (var tree in view)
            {
                visibleTrees++;
                if (tree >= Height)
                    break;
            }

            return visibleTrees;
        }
        
        private void SetVisibilityFromPerimeter(TreeHeightGrid grid)
        {
            if (Location.x == 0
                || Location.y == 0
                || Location.x == grid[Location.y].Length - 1
                || Location.y == grid.Length - 1)
            {
                VisibleFromPerimeter = true;
                return;
            }

            var northView = NorthernView.All(t => t < Height);
            var southView = SouthernView.All(t => t < Height);
            var eastView = EasternView.All(t => t < Height);
            var westView = WesternView.All(t => t < Height);

            VisibleFromPerimeter = eastView || westView || northView || southView;
        }
    }
}