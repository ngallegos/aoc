using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Shouldly;

namespace AOC2023.Tests;

public class Day19Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var input = get_sample().ToList();
        var totalRatingSum = GetTotalRatingSum(input);
        totalRatingSum.ShouldBe(19114);
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        var input = get_input().ToList();
        var totalRatingSum = GetTotalRatingSum(input);
        totalRatingSum.ShouldBe(348378);
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private static int GetTotalRatingSum(List<string> input)
    {
        var workflows = input.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(x => new Workflow(x)).ToList();
        var parts = input.Skip(workflows.Count + 1)
            .Select(x => JsonConvert.DeserializeObject<Part>(x.Replace('=', ':')))
            .ToList();
        var inFlow = workflows.First(x => x.ID == "in");
        var acceptedParts = new List<Part>();
        foreach (var part in parts)
        {
            if (inFlow.CheckPart(part, workflows))
                acceptedParts.Add(part);
        }
        
        return acceptedParts.Sum(x => x.TotalRating);
    }
    private class Part
    {
        public int x { get; set; }
        public int m { get; set; }
        public int a { get; set; }
        public int s { get; set; }
        public int TotalRating => x + m + a + s;
    }

    private class Rule
    {
        private string _definition;
        private string _result;
        private static Regex _definitionRegex = new Regex(@"^(?<prop>[xmas])(?<operator>[\<\>])(?<limit>\d+)$");
        private static PropertyInfo[] _partProps = typeof(Part).GetProperties();
        
        public Rule(string expression)
        {
            var parts = expression.Split(':');
            _result = parts.Last();
            _definition = parts.Length < 2 ? "" : parts.First().Trim();
        }

        public bool Evaluate(Part part)
        {
            if (!string.IsNullOrEmpty(_definition))
            {
                var match = _definitionRegex.Match(_definition);
                var prop = match.Groups["prop"].Value;
                var op = match.Groups["operator"].Value;
                var limit = int.Parse(match.Groups["limit"].Value);
                var propValue = GetPropertyValue(part, prop);
                if (op == ">")
                    return propValue > limit;
                return propValue < limit;
            }

            return true;
        }

        public bool GetOutcome(Part part, List<Workflow> workflows)
        {
            if (_result == "A")
                return true;
            if (_result == "R")
                return false;
            var nextWorkflow = workflows.First(x => x.ID == _result);
            return nextWorkflow.CheckPart(part, workflows);
        }

        private int GetPropertyValue(Part part, string partName)
        {
            var prop = _partProps.First(x => x.Name == partName);
            return (int)prop.GetValue(part);
        }
    }

    private class Workflow
    {
        public string ID { get; private set; }
        public List<Rule> Rules { get; private set; }

        public Workflow(string definition)
        {
            var parts = definition.TrimEnd('}').Split('{');
            ID = parts.First();
            Rules = parts.Last().Split(',').Select(x => new Rule(x)).ToList();
        }
        
        public bool CheckPart(Part part, List<Workflow> workflows)
        {
            var accepted = false;
            foreach (var rule in Rules)
            {
                if (rule.Evaluate(part))
                {
                    accepted = rule.GetOutcome(part, workflows);
                    break;
                }
            }

            return accepted;
        }
    }
}