using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2022.Modules;

public class Day05 : DayBase
{
    
    public override dynamic Part1()
    {
        var (crates, steps) = ParseInput(get_input().ToList());
        steps.ForEach(step =>
        {
            crates.CrateMover9000Rearrange(step);
        });
        return new { topCrates = crates.GetTopCrates() };
    }

    public override dynamic Part2()
    {
        var (crates, steps) = ParseInput(get_input().ToList());
        steps.ForEach(step =>
        {
            crates.CrateMover9001Rearrange(step);
        });
        return new { topCrates = crates.GetTopCrates() };
    }

    private (Crates crates, List<RearrangementStep> steps) ParseInput(List<string> inputs)
    {
        var diagramEnd = inputs.FindIndex(x => string.IsNullOrEmpty(x.Trim()));
        var crates = new Crates(inputs.Take(diagramEnd).ToList());
        var steps = inputs.Skip(diagramEnd + 1)
            .Select(x => new RearrangementStep(x))
            .ToList();
        return (crates, steps);
    }
    
    private class Crates
    {
        private List<Stack<string>> _stacks;
        public Crates(List<string> diagram)
        {
            diagram.Reverse();
            var firstStackIndex = 1;
            var spacesBetweenStacks = 3;
            var numStacks = int.Parse(diagram[0].Trim().Last().ToString());
            diagram.RemoveAt(0);
            _stacks = new List<Stack<string>>();
            var stackNumbers = Enumerable.Range(1, numStacks).ToList();
            _stacks.AddRange(stackNumbers.Select(x => new Stack<string>()));
            foreach (var row in diagram)
            {
                foreach (var stack in stackNumbers.Select((x, i) => new { num = x, i }))
                {
                    var crateContent = row[firstStackIndex + (stack.i * (spacesBetweenStacks + 1))].ToString().Trim();
                    if (!string.IsNullOrEmpty(crateContent))
                        _stacks[stack.i].Push(crateContent);
                }
            }
        }

        public void CrateMover9000Rearrange(RearrangementStep step)
        {
            for (int i = 0; i < step.NumberOfCratesToMove; i++)
            {
                var crate = _stacks[step.SourceStack - 1].Pop();
                _stacks[step.DestinationStack - 1].Push(crate);
            }
        }

        public void CrateMover9001Rearrange(RearrangementStep step)
        {
            var clawContents = new Stack<string>();
            for (int i = 0; i < step.NumberOfCratesToMove; i++)
                clawContents.Push(_stacks[step.SourceStack - 1].Pop());
            while(clawContents.Any())
                _stacks[step.DestinationStack - 1].Push(clawContents.Pop());
            
        }

        public string GetTopCrates()
        {
            var topCrates = string.Empty;
            foreach (var stack in _stacks)
            {
                topCrates += stack.Peek();
            }

            return topCrates;
        }
    }

    private class RearrangementStep
    {
        private static Regex _instructionRegex = new Regex(@"^move (\d+) from (\d+) to (\d+)$");
        public int SourceStack { get; }
        public int DestinationStack { get; }
        public int NumberOfCratesToMove { get; }

        public RearrangementStep(string input)
        {
            var match = _instructionRegex.Match(input);
            NumberOfCratesToMove = int.Parse(match.Groups[1].Value);
            SourceStack = int.Parse(match.Groups[2].Value);
            DestinationStack = int.Parse(match.Groups[3].Value);
        }
    }
}