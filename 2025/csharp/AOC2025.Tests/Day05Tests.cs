namespace AOC2025.Tests;

public class Day05Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var (freshRanges, ingredientIds) = ParseDatabase(get_sample());
        
        // Act
        var freshIngredients = ingredientIds.Count(id => freshRanges.Any(r => r.IsInRange(id)));
        
        // Assert
        freshIngredients.ShouldBe(3);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var (freshRanges, ingredientIds) = ParseDatabase(get_input());
        
        // Act
        var freshIngredients = ingredientIds.Count(id => freshRanges.Any(r => r.IsInRange(id)));
        
        // Assert
        freshIngredients.ShouldBe(615);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var (freshRanges, _) = ParseDatabase(get_sample());
        
        // Act
        var mergedRanges = MergeRanges(freshRanges);
        var freshIngredients = mergedRanges.Sum(r => r.End - r.Start + 1);
        
        // Assert
        freshIngredients.ShouldBe(14);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var (freshRanges, _) = ParseDatabase(get_input());
        
        // Act
        var mergedRanges = MergeRanges(freshRanges);
        var freshIngredients = mergedRanges.Sum(r => r.End - r.Start + 1);
        
        // Assert
        freshIngredients.ShouldBe(353716783056994L);
    }
    
    (IngredientIDRange[] freshRanges, long[] ingredientIds) ParseDatabase(IEnumerable<string> databaseLines)
    {
        var database = databaseLines.ToList();
        var dbSeparator = database.IndexOf("");
        var freshRanges = database[..dbSeparator].Select(r => new IngredientIDRange(r)).ToArray();
        var ingredientIds = database[(dbSeparator + 1)..].Select(long.Parse).ToArray();
        return (freshRanges, ingredientIds);
    }
    
    IngredientIDRange[] MergeRanges(IEnumerable<IngredientIDRange> ranges)
    {
        var sorted = ranges.OrderBy(r => r.Start).ToList();
        var merged = new List<IngredientIDRange>();

        foreach (var r in sorted)
        {
            if (merged.Count == 0)
            {
                merged.Add(new IngredientIDRange($"{r.Start}-{r.End}"));
                continue;
            }

            var last = merged[^1];
            if (r.Start <= last.End) // overlap -> merge
            {
                var mergedEnd = Math.Max(last.End, r.End);
                merged[^1] = new IngredientIDRange($"{last.Start}-{mergedEnd}");
            }
            else
            {
                merged.Add(new IngredientIDRange($"{r.Start}-{r.End}"));
            }
        }

        return merged.ToArray();
    }
    class IngredientIDRange
    {
        public long Start { get; }
        public long End { get; }
        public IngredientIDRange(string range)
        {
            var parts = range.Split('-');
            Start = long.Parse(parts[0]);
            End = long.Parse(parts[1]);
        }

        public bool IsInRange(long id)
        {
            return Start <= id && id <= End;
        }
    }
}