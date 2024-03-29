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
        var maxBound = 4001;
        var minBound = 0;
        
        var workflows = input.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(x => new Workflow(x)).ToList();
        
        var inFlow = workflows.First(x => x.ID == "in");
        var possibleWorkflows = inFlow.FindPossibleWorkflows(workflows);
        var bounds = possibleWorkflows.SelectMany(x => x.Rules)
            .Where(x => (x.HasDefinition && x.ResultAccepts != false) || x.ResultAccepts == true)
            .GroupBy(x => new { x.RatingName, x.HasDefinition })
            .Select(x => new
            {
                x.Key.RatingName, 
                x.Key.HasDefinition,
                Boundaries = !x.Key.HasDefinition ? new { Min = minBound, Max = maxBound }
                    : new { 
                        Min = x.Where(g => !g.LessThan).Select(g => g.RatingLimit).DefaultIfEmpty(minBound).Min(), 
                        Max = x.Where(g => g.LessThan).Select(g => g.RatingLimit).DefaultIfEmpty(maxBound).Max() 
                    }
            })
            .GroupBy(x => x.RatingName)
            .Select(x => new
            {
                RatingName = x.Key,
                Max = x.Select(g => g.Boundaries.Max).Max(),
                Min = x.Select(g => g.Boundaries.Min).Min()
            })
            .ToList();

        long x = bounds.Where(b => b.RatingName == "x")
            .GroupBy(b => b.RatingName)
            .Select(b => new
            {
                Max = b.FirstOrDefault()?.Max ?? maxBound,
                Min = b.FirstOrDefault()?.Min ?? minBound
            }).Select(b => b.Max - b.Min).First();
        long m = bounds.Where(b => b.RatingName == "m")
            .GroupBy(b => b.RatingName)
            .Select(b => new
            {
                Max = b.FirstOrDefault()?.Max ?? maxBound,
                Min = b.FirstOrDefault()?.Min ?? minBound
            }).Select(b => b.Max - b.Min).First();
        long a = bounds.Where(b => b.RatingName == "a")
            .GroupBy(b => b.RatingName)
            .Select(b => new
            {
                Max = b.FirstOrDefault()?.Max ?? maxBound,
                Min = b.FirstOrDefault()?.Min ?? minBound
            }).Select(b => b.Max - b.Min).First();
        long s = bounds.Where(b => b.RatingName == "s")
            .GroupBy(b => b.RatingName)
            .Select(b => new
            {
                Max = b.FirstOrDefault()?.Max ?? maxBound,
                Min = b.FirstOrDefault()?.Min ?? minBound
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
        private static Regex _workflowIDRegex = new Regex(@"^[a-z]+$");
        private static PropertyInfo[] _partProps = typeof(Part).GetProperties();
        private string _resultWorkflowID;
        public string ResultWorkflowID => _resultWorkflowID;
        public bool? ResultAccepts => _result == "A" ? true : _result == "R" ? false : null;
        
        public bool HasDefinition => !string.IsNullOrEmpty(_definition);
        public string RatingName { get; private set; }
        public bool LessThan { get; private set; }
        public int RatingLimit { get; private set; }
        
        public Rule(string expression)
        {
            var parts = expression.Split(':');
            _result = parts.Last();
            _definition = parts.Length < 2 ? "" : parts.First().Trim();
            var workflowMatch = _workflowIDRegex.Match(_result);
            if (workflowMatch.Success)
                _resultWorkflowID = _result;
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

        public List<Workflow> FindPossibleWorkflows(List<Workflow> allWorkFlows, List<Workflow> possibleWorkflows = null)
        {
            possibleWorkflows ??= new List<Workflow>();

            possibleWorkflows.Add(this);
            foreach (var rule in Rules)
            {
                if (!string.IsNullOrEmpty(rule.ResultWorkflowID) && possibleWorkflows.All(w => w.ID != rule.ResultWorkflowID))
                {
                    var nextWorkflow = allWorkFlows.First(x => x.ID == rule.ResultWorkflowID);
                    possibleWorkflows.AddRange(nextWorkflow.FindPossibleWorkflows(allWorkFlows, possibleWorkflows));
                }
            }
            
            return possibleWorkflows.DistinctBy(x => x.ID).ToList();
        }
    }
}