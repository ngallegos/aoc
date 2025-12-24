namespace AOC2025.Tests;

public class Day06Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var worksheet = get_sample().ToList();
        
        // Act
        var runningTotals = GetRunningTotals_RowLeftToRight(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(4277556L);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var worksheet = get_input().ToList();
        
        // Act
        var runningTotals = GetRunningTotals_RowLeftToRight(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(5381996914800L);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var worksheet = get_sample().ToList();
        
        // Act
        var runningTotals = GetRunningTotals_ColumnRightToLeft(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(3263827L);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var worksheet = get_input().ToList();
        
        // Act
        var runningTotals = GetRunningTotals_ColumnRightToLeft(worksheet);
        
        var grandTotal = runningTotals.Sum();
        
        // Assert
        grandTotal.ShouldBe(9627174150897L);
    }

    long[] GetRunningTotals_RowLeftToRight(List<string> worksheet)
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
                        throw new Exception($"Unknown operator {operators[j]}");
                }
            }
        }
        
        return runningTotals;
    }
    
    long[] GetRunningTotals_ColumnRightToLeft(List<string> worksheet)
    {
        var operatorLine = worksheet.Last();
        var operators = operatorLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
        var columnWidths = new int[operators.Length];

        var col = 0;
        foreach (var c in operatorLine.Skip(1))
        {
            if (c != ' ')
                col++;
            else
                columnWidths[col]++;
        }

        columnWidths[^1]++;

        var currentIndex = 0;
        var equations = new int[columnWidths.Length][];
        foreach (var cw in columnWidths.Select((x, i) => new { width = x, ix = i }))
        {
            var numbers = new int[cw.width];

            for (var j = cw.width-1; j >= 0; j--)
            {
                var numString = "";
                for (var i = 0; i < worksheet.Count - 1; i++)
                {
                    numString += worksheet[i][j + currentIndex];
                }
                
                numbers[j] = int.Parse(numString.Replace(" ", ""));
            }
            
            equations[cw.ix] = numbers;
            currentIndex += cw.width + 1;
        }
        
        var runningTotals = new long[columnWidths.Length];

        foreach (var eq in equations.Select((x, i) => new { equation = x, ix = i }))
        {
            var op = operators[eq.ix];
            foreach (var n in eq.equation.Select((x, i) => new { num = x, ix = i }))
            {
                switch (op)
                {
                    case "+":
                        if (n.ix == 0)
                            runningTotals[eq.ix] = 0;
                        runningTotals[eq.ix] += n.num;
                        break;
                    case "*":
                        if (n.ix == 0)
                            runningTotals[eq.ix] = 1;
                        runningTotals[eq.ix] *= n.num;
                        break;
                    default:
                        throw new Exception($"Unknown operator {op}");
                }
            }
        }
        
        return runningTotals;
    }
}
