using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2015.Modules;

public class Day05 : DayBase
{
    public override bool Ignore => true;
    private Regex vowelsRegex = new Regex(@"[aeiou]");
    private Regex doublesRegex = new Regex(@"(.{1})\1");
    private Regex naughtyStringsRegex = new Regex(@"ab|cd|pq|xy");
    private Regex repeatingPairRegex = new Regex(@"(.{2}).*\1");
    private Regex sandwichRegex = new Regex(@"(.{1})[^\1]{1}\1");
    public override dynamic Part1()
    {
        var theList = get_input().ToList();
        var niceCount = theList.Count(IsNicePart1);

        return new { niceCount };
    }

    public override dynamic Part2()
    {
        var theList = get_input().ToList();
        var niceCount = theList.Count(IsNicePart2);

        return new { niceCount };
    }

    private bool IsNicePart1(string str)
    {
        return vowelsRegex.Matches(str).Count >= 3
               && doublesRegex.IsMatch(str)
               && !naughtyStringsRegex.IsMatch(str);
    }

    private bool IsNicePart2(string str)
    {
        return repeatingPairRegex.IsMatch(str) && sandwichRegex.IsMatch(str);
    }
}