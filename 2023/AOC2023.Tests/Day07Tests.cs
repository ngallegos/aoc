using Shouldly;

namespace AOC2023.Tests;

public class Day07Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var hands = get_sample(x => new Hand(x))
            .ToList();
        var totalWinnings = FindTotalWinnings(hands);
        totalWinnings.ShouldBe(6440L);
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        var hands = get_input(x => new Hand(x))
            .ToList();
        var totalWinnings = FindTotalWinnings(hands);
        totalWinnings.ShouldBe(6440L);
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private long FindTotalWinnings(List<Hand> hands)
    {
        var ordered = hands.OrderBy(x => x, new HandComparer())
            .Select((x, i) =>
            {
                x.SetRank(i + 1);
                return x;
            });
        return ordered
            .Sum(x => x.Winnings);
    }
    
    protected class HandComparer : IComparer<Hand>
    {
        public int Compare(Hand x, Hand y)
        {
            if (x.Type > y.Type)
                return 1;
            if (x.Type < y.Type)
                return -1;
            for (int i = 0; i < x.Cards.Length; i++)
            {
                if (_cardValueMap[x.Cards[i]] > _cardValueMap[y.Cards[i]])
                    return 1;
                if (_cardValueMap[x.Cards[i]] < _cardValueMap[y.Cards[i]])
                    return -1;
            }

            return 0;
        }
    }
    protected class Hand
    {
        public HandType Type { get; private set; }
        public char[] Cards { get; private set; }
        public long Bid { get; private set; }
        public long Rank { get; private set; }
        public long Winnings => Bid * Rank;
        
        public Hand(string input)
        {
            var split = input.Split(' ');
            Bid = long.Parse(split[1]);
            Cards = split[0].ToCharArray();
            DetermineHandType();
        }
        
        private void DetermineHandType()
        {
            if (Cards.Distinct().Count() == 1)
                Type = HandType.FiveOfAKind;
            else if (Cards.Distinct().Count() == 2)
            {
                if (Cards.GroupBy(x => x).Any(x => x.Count() == 4))
                    Type = HandType.FourOfAKind;
                else
                    Type = HandType.FullHouse;
            }
            else if (Cards.Distinct().Count() == 3)
            {
                if (Cards.GroupBy(x => x).Any(x => x.Count() == 3))
                    Type = HandType.ThreeOfAKind;
                else
                    Type = HandType.TwoPair;
            }
            else if (Cards.Distinct().Count() == 4)
                Type = HandType.OnePair;
            else
                Type = HandType.HighCard;
        }
        
        public void SetRank(long rank)
        {
            Rank = rank;
        }
    }

    protected static Dictionary<char, int> _cardValueMap = new Dictionary<char, int>
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    protected enum HandType
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }
}