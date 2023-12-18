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
        step.ShouldBe(19637L);
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private long  FindStepsToZZZ(List<string> input)
    {
        var instructions = input[0].ToCharArray();
        var nodes = input.Skip(2)
            .Select(x => new Node(x))
            .ToList();
        nodes.ForEach(x => x.SetNodeLinks(nodes));
        var currentNode = nodes.First(x => x.ID == "AAA");
        var step = 0L;
        var totalInstructions = instructions.Length;
        while (currentNode.ID != "ZZZ")
        {
            var instruction = instructions[ step % totalInstructions];
            if (instruction == 'L')
                currentNode = currentNode.Left;
            else
                currentNode = currentNode.Right;
            step++;
        }

        return step;
    }
    
    private class Node
    {
        public string ID { get; private set; }
        public string LeftID { get; private set; }
        public string RightID { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }
        public Node(string definition)
        {
            var parts = definition.Split('=', StringSplitOptions.TrimEntries);
            ID = parts[0];
            var destinations = parts[1].TrimStart('(').TrimEnd(')').Split(',', StringSplitOptions.TrimEntries);
            LeftID = destinations[0];
            RightID = destinations[1];
        }
        
        public void SetNodeLinks(List<Node> nodes)
        {
            Left = nodes.First(x => x.ID == LeftID);
            Right = nodes.First(x => x.ID == RightID);
        }
    }
}