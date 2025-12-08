namespace AOC2025.Tests;

public class Day06Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var worksheet = get_sample().ToList();
        
        // Act
        var runningTotals = GetRunningTotals(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(4277556L);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var worksheet = get_input().ToList();
        
        // Act
        var runningTotals = GetRunningTotals(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(5381996914800L);
    }

    protected override void SolvePart2_Sample()
    {
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new System.NotImplementedException();
    }

    long[] GetRunningTotals(List<string> worksheet)
    {
        var operators = worksheet.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
        var numbers = worksheet[..^1].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();

        var runningTotals = new long[numbers[0].Length];
        
        for (int i = 0; i < numbers.Length; i++)
        {
            for (int j = 0; j < runningTotals.Length; j++)
            {
                switch (operators[j])
                {
                    case "+":
                        if (i == 0)
                            runningTotals[j] = 0;
                        runningTotals[j] += numbers[i][j];
                        break;
                    case "*":
                        if (i == 0)
                            runningTotals[j] = 1;
                        runningTotals[j] *= numbers[i][j];
                        break;
                    default:
                        throw new System.Exception($"Unknown operator {operators[j]}");
                }
            }
        }
        
        return runningTotals;
    }
}
