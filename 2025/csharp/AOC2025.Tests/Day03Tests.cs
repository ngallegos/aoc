namespace AOC2025.Tests;

public class Day03Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var input = get_sample()
            .Select(x => new Bank(x))
            .ToArray();
        
        // Act
        var totalJoltage = input.Sum(b => b.MaxJoltage);
        
        // Assert
        input.ShouldNotBeEmpty();
        totalJoltage.ShouldBe(357L);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var input = get_input()
            .Select(x => new Bank(x))
            .ToArray();
        
        // Act
        var totalJoltage = input.Sum(b => b.MaxJoltage);
        
        // Assert
        input.ShouldNotBeEmpty();
        totalJoltage.ShouldBe(17166L);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var input = get_sample()
            .Select(x => new Bank(x, 12))
            .ToArray();
        
        // Act
        var totalJoltage = input.Sum(b => b.MaxJoltage);
        
        // Assert
        input.ShouldNotBeEmpty();
        totalJoltage.ShouldBe(3121910778619L);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var input = get_input()
            .Select(x => new Bank(x, 12))
            .ToArray();
        
        // Act
        var totalJoltage = input.Sum(b => b.MaxJoltage);
        
        // Assert
        input.ShouldNotBeEmpty();
        totalJoltage.ShouldBe(169077317650774L);
    }

    class Bank
    {
        public long MaxJoltage { get; } = 0;

        public Bank(string bank, int batteriesToProcess = 2)
        {
            var batteries = bank[..].Select(b => (int)char.GetNumericValue(b)).ToArray();
            
            MaxJoltage = FindMaxJoltage(batteries, batteriesToProcess);
        }
        
        private long FindMaxJoltage(int[] batteries, int batteriesToProcess)
        {
            var max = batteries.Max();
            if (batteriesToProcess == 1)
            {
                return max;
            }
            
            var indexOfMaximum = Array.IndexOf(batteries, max);
            var remainingBatteries = batteriesToProcess - 1;
            var lastValidIndex = batteries.Length - remainingBatteries - 1;
            var multiplier = (long)Math.Pow(10, remainingBatteries);
            if (indexOfMaximum > lastValidIndex)
            {
                var validBatteries = batteries[..^remainingBatteries];
                max = validBatteries.Max();
                indexOfMaximum = Array.IndexOf(validBatteries, max);
                return max * multiplier + FindMaxJoltage(batteries[(indexOfMaximum+1)..], remainingBatteries);
            }
            
            return max * multiplier + FindMaxJoltage(batteries[(indexOfMaximum+1)..], remainingBatteries);
        }
    }
}