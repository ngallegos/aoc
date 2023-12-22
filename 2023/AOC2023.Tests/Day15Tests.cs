using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day15Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var hashSum = get_sample(x => x.Split(',').Select(c => c.ComputeHash()))
            .SelectMany(x => x)
            .Sum();
        hashSum.ShouldBe(1320);
    }

    protected override void SolvePart1_Actual()
    {
        var hashSum = get_input(x => x.Split(',').Select(c => c.ComputeHash()))
            .SelectMany(x => x)
            .Sum();
        hashSum.ShouldBe(518107);
    }

    protected override void SolvePart2_Sample()
    {
        var steps = get_sample(x => x.Split(',').Select(i => new Step(i)))
            .SelectMany(x => x)
            .ToList();
        var totalFocusingPower = GetTotalFocusingPower(steps);
        totalFocusingPower.ShouldBe(145);
    }

    protected override void SolvePart2_Actual()
    {
        var steps = get_input(x => x.Split(',').Select(i => new Step(i)))
            .SelectMany(x => x)
            .ToList();
        var totalFocusingPower = GetTotalFocusingPower(steps);
        totalFocusingPower.ShouldBe(303404);
    }

    private int GetTotalFocusingPower(List<Step> steps)
    {
        var lenses = new List<Lens>();
        foreach (var step in steps)
            step.Process(lenses);
        var boxes = lenses.GroupBy(x => x.BoxID)
            .Select(x => new
            {
                BoxID = x.Key,
                FocusingPower = x.Select((lens, index) => lens.GetFocusingPower(index + 1))
                    .DefaultIfEmpty(0).Sum()
            });
        var totalFocusingPower = boxes.Sum(x => x.FocusingPower);
        return totalFocusingPower;
    }

    private class Step
    {
        private static Regex _regex = new Regex(@"^(?<label>[a-z]+)(?<op>[=-])(?<flength>\d?)$");
        public string Label { get; private set; }
        public char Operator { get; private set; }
        public int? FocalLength { get; private set; }

        public Step(string input)
        {
            var match = _regex.Match(input);
            if (!match.Success)
                throw new ArgumentException($"Invalid input: {input}");
            Label = match.Groups["label"].Value;
            Operator = match.Groups["op"].Value[0];
            if (int.TryParse(match.Groups["flength"].Value, out var focalLength))
                FocalLength = focalLength;
        }

        public void Process(List<Lens> lenses)
        {
            if (Operator == '=')
            {
                var existingLens = lenses.FirstOrDefault(x => x.Label == Label);
                if (existingLens == null)
                    lenses.Add(new Lens(Label, FocalLength.Value));
                else
                    lenses[lenses.IndexOf(existingLens)] = new Lens(Label, FocalLength.Value);
            }
            else
                lenses.RemoveAll(x => x.Label == Label);
        }
    }

    private class Lens
    {
        public string Label { get; private set; }
        public int FocalLength { get; private set; }
        public int BoxID => Label.ComputeHash();
        public Lens (string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
        }

        public int GetFocusingPower(int order)
        {
            return (BoxID + 1) * order * FocalLength;
        }
    }
}

public static class Day15Extensions 
{
    public static int ComputeHash(this string input)
    {
        var hash = 0;
        foreach (var c in input)
        {
            hash += (int)c;
            hash *= 17;
            hash = hash % 256;
        }

        return hash;
    }
}