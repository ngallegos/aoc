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
        var hands = get_sample(x => new Hand(x, true))
            .ToList();
        var totalWinnings = FindTotalWinnings(hands);
        totalWinnings.ShouldBe(5905L);
    }

    protected override void SolvePart1_Actual()
    {
        var hands = get_input(x => new Hand(x))
            .ToList();
        var totalWinnings = FindTotalWinnings(hands);
        totalWinnings.ShouldBe(253638586L);
    }

    protected override void SolvePart2_Actual()
    {
        var hands = get_input(x => new Hand(x, true))
            .ToList();
        var totalWinnings = FindTotalWinnings(hands);
        totalWinnings.ShouldBe(253638586L);
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
        
        public Hand(string input, bool jackIsWild = false)
        {
            var split = input.Split(' ');
            Bid = long.Parse(split[1]);
            Cards = split[0].ToCharArray();
            DetermineHandType(jackIsWild);
        }
        
        private void DetermineHandType(bool jackIsWild = false)
        {
            var groups = Cards.GroupBy(x => x)
                .Select(g => new { isWild = g.Key == 'J' && jackIsWild, g.Key, count = g.Count() })
                .ToList();

            var wildGroup = groups.FirstOrDefault(x => x.isWild);
            
            if (groups.Count == 1)
                Type = HandType.FiveOfAKind;
            else if (groups.Count == 2)
            {
                if (groups.Any(x => x.count == 4))
                    if (wildGroup == null)
                        Type = HandType.FourOfAKind;
                    else
                        Type = HandType.FiveOfAKind;
                else if (wildGroup == null)
                    Type = HandType.FullHouse;
                else
                    Type = HandType.FiveOfAKind;
            }
            else if (groups.Count == 3)
            {
                if (groups.Any(x => x.count == 3))
                {
                    if (wildGroup == null)
                        Type = HandType.ThreeOfAKind;
                    else
                        Type = HandType.FourOfAKind;
                }
                else if (wildGroup == null)
                    Type = HandType.TwoPair;
                else if (wildGroup.count == 2)
                    Type = HandType.FourOfAKind;
                else
                    Type = HandType.FullHouse;
            }
            else if (groups.Count == 4)
            {
                if (wildGroup != null)
                    Type = HandType.ThreeOfAKind;
                else
                    Type = HandType.OnePair;
            }
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
        { '2', 2 },
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