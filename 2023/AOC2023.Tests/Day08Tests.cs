using Shouldly;

namespace AOC2023.Tests;

public class Day08Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var input = get_sample().ToList();
        var step = FindStepsToZZZ(input);
        step.ShouldBe(6);
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        var input = get_input().ToList();
        var step = FindStepsToZZZ(input);
        step.ShouldBe(6);
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private int FindStepsToZZZ(List<string> input)
    {
        var instructions = input[0].ToCharArray();
        var nodes = input.Skip(2)
            .Select(x => new Node(x))
            .ToList();
        var currentNode = nodes[0];
        var step = 0;
        var totalInstructions = instructions.Length;
        while (currentNode.ID != "ZZZ")
        {
            var instruction = instructions[ step % totalInstructions];
            if (instruction == 'L')
                currentNode = nodes.First(x => x.ID == currentNode.Left);
            else
                currentNode = nodes.First(x => x.ID == currentNode.Right);
            step++;
        }

        return step;
    }
    
    private class Node
    {
        public string ID { get; private set; }
        public string Left { get; private set; }
        public string Right { get; private set; }
        public Node(string definition)
        {
            var parts = definition.Split('=', StringSplitOptions.TrimEntries);
            ID = parts[0];
            var destinations = parts[1].TrimStart('(').TrimEnd(')').Split(',', StringSplitOptions.TrimEntries);
            Left = destinations[0];
            Right = destinations[1];
        }
    }
}