using System.Text.RegularExpressions;

namespace AOC2015.Tests;

public class Day05Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var strings = get_sample().ToList();

        // Act
        var niceCount = strings.Count(IsNicePart1);

        // Assert
        niceCount.ShouldBe(2);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var strings = get_input().ToList();

        // Act
        var niceCount = strings.Count(IsNicePart1);

        // Assert
        niceCount.ShouldBe(238);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var strings = get_sample(2).ToList();

        // Act
        var niceCount = strings.Count(IsNicePart2);

        // Assert
        niceCount.ShouldBe(2);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var strings = get_input().ToList();

        // Act
        var niceCount = strings.Count(IsNicePart2);

        // Assert
        niceCount.ShouldBe(69);
    }

    private Regex vowelsRegex = new Regex(@"[aeiou]");
    private Regex doublesRegex = new Regex(@"(.{1})\1");
    private Regex naughtyStringsRegex = new Regex(@"ab|cd|pq|xy");
    private Regex repeatingPairRegex = new Regex(@"(.{2}).*\1");
    private Regex sandwichRegex = new Regex(@"(.{1})[^\1]{1}\1");
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
