using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2015.Modules.Shared;

namespace AOC2015.Modules;

public class Day06 : DayBase
{
    public override bool Completed => true;

    private static Regex _instructionRegex =
        new Regex(@"^(?<type>turn|toggle) ?(?<state>on|off)? (?<start>\d*,\d*) through (?<end>\d*,\d*)$");

    private List<LightingInstruction> _instructions = null;
    public override dynamic Part1()
    {
        _instructions ??= get_input().Select(x => new LightingInstruction(x))
            .ToList();
        var grid = new LightGrid();
        foreach (var instruction in _instructions)
        {
            instruction.ExecutePart1(grid);
        }

        return grid;
    }

    public override dynamic Part2()
    {
        _instructions ??= get_input().Select(x => new LightingInstruction(x))
            .ToList();
        var grid = new LightGrid();
        foreach (var instruction in _instructions)
        {
            instruction.ExecutePart2(grid);
        }

        return grid;
    }

    private class LightGrid : Grid<int>
    {
        public LightGrid()
        {
            Initialize(new GridLocation(0, 0), new GridLocation(999, 999));
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