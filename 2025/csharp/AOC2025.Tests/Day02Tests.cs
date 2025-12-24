using System.Text.RegularExpressions;

namespace AOC2025.Tests;

public class Day02Tests : TestBase
{
    static Regex _rangeRegex = new(@"(\d+)-(\d+)", RegexOptions.Compiled);
    static Regex _repeatingOnceRegex = new (@"^(\d+)\1$", RegexOptions.Compiled);
    static Regex _repeatingAtLeastOnceRegex = new (@"^(\d+?)\1+$", RegexOptions.Compiled);
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var input = get_sample();
        var ranges = input.FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => new ProductIdRange(r, _repeatingOnceRegex))
            .ToArray() ?? [];
        
        // Act
        var totalInvalid = ranges.SelectMany(r => r.InvalidIDs).Sum();
        
        // Assert
        input.ShouldNotBeEmpty();
        totalInvalid.ShouldBe(1227775554L);
    }

    protected override void SolvePart1_Actual()
    {
        var input = get_input();
        var ranges = input.FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => new ProductIdRange(r, _repeatingOnceRegex))
            .ToArray() ?? [];
        
        // Act
        var totalInvalid = ranges.SelectMany(r => r.InvalidIDs).Sum();
        
        // Assert
        input.ShouldNotBeEmpty();
        totalInvalid.ShouldBe(20223751480L);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var input = get_sample();
        var ranges = input.FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => new ProductIdRange(r, _repeatingAtLeastOnceRegex))
            .ToArray() ?? [];
        
        // Act
        var totalInvalid = ranges.SelectMany(r => r.InvalidIDs).Sum();
        
        // Assert
        input.ShouldNotBeEmpty();
        totalInvalid.ShouldBe(4174379265L);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var input = get_input();
        var ranges = input.FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => new ProductIdRange(r, _repeatingAtLeastOnceRegex))
            .ToArray() ?? [];
        
        // Act
        var totalInvalid = ranges.SelectMany(r => r.InvalidIDs).Sum();
        
        // Assert
        input.ShouldNotBeEmpty();
        totalInvalid.ShouldBe(30260171216L);;
    }

    class ProductIdRange
    {
        long Start { get; }
        long End { get; }
        string[] IDs { get; }
        public long[] InvalidIDs { get; }
        
        public ProductIdRange(string range, Regex invalidIDRegex)
        {
            var match = _rangeRegex.Match(range);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid range format", nameof(range));
            }
            Start = long.Parse(match.Groups[1].Value);
            End = long.Parse(match.Groups[2].Value);
            var ids = new List<long>();
            var current = Start;
            while (current <= End)
            {
                ids.Add(current);
                current++;
            }
            IDs = ids.Select(r => r.ToString()).ToArray();
            InvalidIDs = IDs.Where(id => invalidIDRegex.IsMatch(id)).Select(long.Parse).ToArray();
        }
    }
}