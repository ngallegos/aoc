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

    protected override void SolvePart1_Actual()
    {
        var input = get_input().ToList();
        var totalRatingSum = GetTotalRatingSum(input);
        totalRatingSum.ShouldBe(348378);
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

    protected override void SolvePart2_Sample()
    {
        var input = get_sample().ToList();
        var totalRatingSum = GetAcceptableCombinations(input);
        totalRatingSum.ShouldBe(167409079868000L);
    }
    
    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }
    
    private static long GetAcceptableCombinations(List<string> input)
    {
        var workflows = input.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(x => new Workflow(x)).ToList();
        var bounds = workflows.SelectMany(x => x.Rules)
            .Where(x => x.HasDefinition)
            .GroupBy(x => new { x.RatingName, x.LessThan })
            .Select(x => new
            {
                x.Key.RatingName, 
                x.Key.LessThan,
                Boundary = x.Key.LessThan ? x.Max(g => Math.Min(g.RatingLimit, 4000)) : x.Min(g => Math.Max(g.RatingLimit, 1))
            })
            .ToList();
        
        long x = bounds.GroupBy(b => b.RatingName == "x")
            .Select(b => new
            {
                Max = b.First(g => g.LessThan).Boundary,
                Min = b.First(g => !g.LessThan).Boundary
            }).Select(b => b.Max - b.Min).First();
        long m = bounds.GroupBy(b => b.RatingName == "m")
            .Select(b => new
            {
                Max = b.First(g => g.LessThan).Boundary,
                Min = b.First(g => !g.LessThan).Boundary
            }).Select(b => b.Max - b.Min).First();
        long a = bounds.GroupBy(b => b.RatingName == "a")
            .Select(b => new
            {
                Max = b.First(g => g.LessThan).Boundary,
                Min = b.First(g => !g.LessThan).Boundary
            }).Select(b => b.Max - b.Min).First();
        long s = bounds.GroupBy(b => b.RatingName == "s")
            .Select(b => new
            {
                Max = b.First(g => g.LessThan).Boundary,
                Min = b.First(g => !g.LessThan).Boundary
            }).Select(b => b.Max - b.Min).First();

        return x * m * a * s;
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
        
        public bool HasDefinition => !string.IsNullOrEmpty(_definition);
        public string RatingName { get; private set; }
        public bool LessThan { get; private set; }
        public int RatingLimit { get; private set; }
        
        public Rule(string expression)
        {
            var parts = expression.Split(':');
            _result = parts.Last();
            _definition = parts.Length < 2 ? "" : parts.First().Trim();
            if (HasDefinition)
            {
                var match = _definitionRegex.Match(_definition);
                RatingName = match.Groups["prop"].Value;
                var op = match.Groups["operator"].Value;
                LessThan = op == "<";
                RatingLimit = int.Parse(match.Groups["limit"].Value);
            }
        }

        public bool Evaluate(Part part)
        {
            if (HasDefinition)
            {
                var propValue = GetPropertyValue(part, RatingName);
                if (LessThan)
                    return propValue < RatingLimit;
                return propValue > RatingLimit;
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