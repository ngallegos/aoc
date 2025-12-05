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
        totalJoltage.ShouldBe(357);
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
        totalJoltage.ShouldBe(17166);
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    class Bank
    {
        private int[] batteries;
        public int MaxJoltage { get; } = 0;

        public Bank(string bank)
        {
            batteries = bank[..].Select(b => (int)char.GetNumericValue(b)).ToArray();
            var max = batteries.Max();
            var maxIndex = Array.IndexOf(batteries, max);
            if (maxIndex == batteries.Length - 1)
            {
                MaxJoltage = batteries[..^1].Max() * 10 + max;
            }
            else
            {
                MaxJoltage = max * 10 + batteries[(maxIndex+1)..].Max();
            }
        }
    }
}