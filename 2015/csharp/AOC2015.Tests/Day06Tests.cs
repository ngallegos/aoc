using System.Text.RegularExpressions;
using AOC2015.Tests.Shared;

namespace AOC2015.Tests;

public class Day06Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var instructions = get_sample().Select(x => new LightingInstruction(x)).ToList();
        var grid = new LightGrid();

        // Act
        instructions.ForEach(x => x.ExecutePart1(grid));

        // Assert
        grid.LightsLit.ShouldBe(998996);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var instructions = get_input().Select(x => new LightingInstruction(x)).ToList();
        var grid = new LightGrid();

        // Act
        instructions.ForEach(x => x.ExecutePart1(grid));

        // Assert
        grid.LightsLit.ShouldBe(400410);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var instructions = get_sample(2).Select(x => new LightingInstruction(x)).ToList();
        var grid = new LightGrid();

        // Act
        instructions.ForEach(x => x.ExecutePart2(grid));

        // Assert
        grid.LightsLit.ShouldBe(2000001);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var instructions = get_input().Select(x => new LightingInstruction(x)).ToList();
        var grid = new LightGrid();

        // Act
        instructions.ForEach(x => x.ExecutePart2(grid));

        // Assert
        grid.LightsLit.ShouldBe(15343601);
    }

    private static readonly Regex _instructionRegex =
        new Regex(@"^(?<type>turn|toggle) ?(?<state>on|off)? (?<start>\d*,\d*) through (?<end>\d*,\d*)$");

    private class LightGrid : Grid<int>
    {
        public LightGrid()
        {
            Initialize(new GridLocation(0, 0), new GridLocation(999, 999), 0);
        }

        public int LightsLit => _grid.Sum(x => x.Sum());
    }

    private class LightingInstruction
    {
        private enum Action
        {
            On,
            Off,
            Toggle
        }
        
        private Action _action { get; }
        private List<(int x, int y)> _targets { get; }
        
        public LightingInstruction(string input)
        {
            var result = _instructionRegex.Match(input);
            var type = result.Groups["type"].Value;
            var state = result.Groups["state"].Value;
            var startPoint = result.Groups["start"].Value.Split(',').Select(int.Parse).ToArray();
            var endPoint = result.Groups["end"].Value.Split(',').Select(int.Parse).ToArray();

            if (type == "toggle")
                _action = Action.Toggle;
            else if (state == "on")
                _action = Action.On;
            else
                _action = Action.Off;

            _targets = new List<(int x, int y)>();
            for (int y = startPoint[1]; y <= endPoint[1]; y++)
            {
                for (int x = startPoint[0]; x <= endPoint[0]; x++)
                    _targets.Add((x, y));
            }
        }

        public void ExecutePart1(LightGrid grid)
        {
            foreach (var target in _targets)
            {
                switch (_action)
                {
                    case Action.On:
                        grid[target.y][target.x] = 1;
                        break;
                    case Action.Off:
                        grid[target.y][target.x] = 0;
                        break;
                    case Action.Toggle:
                        grid[target.y][target.x] = (grid[target.y][target.x] + 1) % 2;
                        break;
                }
            }
        }
        
        public void ExecutePart2(LightGrid grid)
        {
            foreach (var target in _targets)
            {
                switch (_action)
                {
                    case Action.On:
                        grid[target.y][target.x] += 1;
                        break;
                    case Action.Off:
                        grid[target.y][target.x] = Math.Max(grid[target.y][target.x] - 1, 0);
                        break;
                    case Action.Toggle:
                        grid[target.y][target.x] += 2;
                        break;
                }
            }
        }
    }
}
