using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day04Tests : TestBase
{
    [Test]
    public override void Part1_Sample()
    {
        var cards = get_sample()
            .Select(x => new Card(x)).ToList();
        
        var totalPoints = cards.Sum(x => x.PointValue);
        
        totalPoints.ShouldBe(13);
    }
    
    [Test]
    public override void Part1_Actual()
    {
        var cards = get_input()
            .Select(x => new Card(x)).ToList();
        
        var totalPoints = cards.Sum(x => x.PointValue);
        
        totalPoints.ShouldBe(18619);
    }

    private class Card
    {
        private static readonly Regex _cardRegex = new(@"Card[ ]+(?<cardNumber>[\d]+): (?<winning>[^|]+) \| (?<mine>.*)", RegexOptions.Compiled);
        private List<long> _winningNumbers = new();
        private List<long> _myNumbers = new();
        private List<long> _matchedNumbers = new();
        private int _cardNumber;
        
        public long PointValue => (long)Math.Pow(2, _matchedNumbers.Count - 1);
        
        public Card(string input)
        {
            var match = _cardRegex.Match(input);
            _cardNumber = int.Parse(match.Groups["cardNumber"].Value);
            _winningNumbers.AddRange(match.Groups["winning"].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
            _myNumbers.AddRange(match.Groups["mine"].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
            _matchedNumbers.AddRange(_winningNumbers.Intersect(_myNumbers));
        }
    }
}