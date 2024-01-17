using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day04Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var cards = get_sample()
            .Select(x => new Card(x)).ToList();
        
        var totalPoints = cards.Sum(x => x.PointValue);
        
        totalPoints.ShouldBe(13);
    }
    
    protected override void SolvePart1_Actual()
    {
        var cards = get_input()
            .Select(x => new Card(x)).ToList();
        
        var totalPoints = cards.Sum(x => x.PointValue);
        
        totalPoints.ShouldBe(18619);
    }
    
    protected override void SolvePart2_Sample()
    {
        var cards = get_sample()
            .Select(x => new Card(x))
            .Reverse().ToList();
        
        var processedCards = new Stack<Card>();

        foreach (var card in cards)
        {
            card.ProcessCardsWon(processedCards);
            processedCards.Push(card);
        }
        
        var totalCards = cards.Sum(x => x.CardsWon);
        
        totalCards.ShouldBe(30);
    }
    
    protected override void SolvePart2_Actual()
    {
        var cards = get_input()
            .Select(x => new Card(x))
            .Reverse().ToList();
        
        var processedCards = new Stack<Card>();

        foreach (var card in cards)
        {
            card.ProcessCardsWon(processedCards);
            processedCards.Push(card);
        }
        
        var totalCards = cards.Sum(x => x.CardsWon);
        
        totalCards.ShouldBe(8063216);
    }

    private class Card
    {
        private static readonly Regex _cardRegex = new(@"Card[ ]+(?<cardNumber>[\d]+): (?<winning>[^|]+) \| (?<mine>.*)", RegexOptions.Compiled);
        private List<long> _winningNumbers = new();
        private List<long> _myNumbers = new();
        private List<long> _matchedNumbers = new();
        private int _cardNumber;

        private long _cardsWon = 1;
        
        public long PointValue => (long)Math.Pow(2, WinningMatches - 1);
        public int WinningMatches => _matchedNumbers.Count;
        public long CardsWon => _cardsWon;
        public Card(string input)
        {
            var match = _cardRegex.Match(input);
            _cardNumber = int.Parse(match.Groups["cardNumber"].Value);
            _winningNumbers.AddRange(match.Groups["winning"].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
            _myNumbers.AddRange(match.Groups["mine"].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
            _matchedNumbers.AddRange(_winningNumbers.Intersect(_myNumbers));
        }
        
        public void ProcessCardsWon(IEnumerable<Card> allCardsBelow)
        {
            var cardsBelow = allCardsBelow.Take(WinningMatches).ToList();
            foreach (var card in cardsBelow)
                _cardsWon += card.CardsWon;
            
        }
    }
}