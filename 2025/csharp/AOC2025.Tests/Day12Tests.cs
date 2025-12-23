namespace AOC2025.Tests;

public class Day12Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var summary = get_sample().ToList();

        var (gifts, treeRegions) = Parse(summary);
        
        // Act
        var bigEnoughRegions = treeRegions.Where(x => x.IsBigEnough).ToArray();
        
        // Assert
        bigEnoughRegions.Length.ShouldBe(3);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var summary = get_input().ToList();

        var (gifts, treeRegions) = Parse(summary);
        
        // Act
        var bigEnoughRegions = treeRegions.Where(x => x.IsBigEnough).ToArray();
        
        // Assert
        bigEnoughRegions.Length.ShouldBe(589);
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    (List<Gift> gifts, List<TreeRegion> treeRegions) Parse(List<string> summary)
    {
        
        var giftDefinitions = summary.TakeWhile(x => !x.Split(':')[0].Contains('x')).ToList();
        var gifts = new List<Gift>();
        var currentGiftId = -1;
        var currentGiftShape = new List<string>();
        foreach (var gDef in giftDefinitions)
        {
            if (gDef.EndsWith(':'))
            {
                currentGiftId = int.Parse(gDef.TrimEnd(':'));
                currentGiftShape.Clear();
                continue;
            }

            if (gDef.Trim() == string.Empty && currentGiftId > -1)
            {
                gifts.Add(new Gift(currentGiftId, currentGiftShape));
            }
        }
        
        var treeRegions = summary.Skip(giftDefinitions.Count)
            .Select(x => new TreeRegion(x, gifts))
            .ToList();

        return (gifts, treeRegions);
    }

    record Gift(int Id, List<string> Shape)
    {
        public int Area { get; } = Shape.SelectMany(x => x).Count(x => x == '#');
    }

    record TreeRegion
    {
        public TreeRegion(string definition, List<Gift> gifts)
        {
            var defParts = definition.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var dims = defParts[0].Split('x');
            
            Width = int.Parse(dims[0]);
            Length = int.Parse(dims[1]);
            Area = Length * Width;
            
            GiftCounts = defParts[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray();
            
            var totalGifts = GiftCounts.Sum();
            var maxGiftArea = totalGifts * 8;
            if (maxGiftArea <= Area)
                IsBigEnough = true;
        }
        
        
        public bool IsBigEnough { get; }
        int Width { get; }
        int Length { get; }
        long Area { get; }
        int[] GiftCounts { get; }
    }
}
