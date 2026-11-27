namespace AOC2015.Tests;

public class Day10Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var sequence = get_sample().ToList()[0];
        
        // Act
        sequence = LookAndSay(sequence);
        
        // Assert
        sequence.ShouldBe("312211");
        sequence.Length.ShouldBe(6);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var sequence = get_input().ToList()[0];
        
        // Act
        sequence = LookAndSay(sequence, 40);
        
        // Assert
        //sequence.ShouldBe("312211");
        sequence.Length.ShouldBe(360154);
        //Console.WriteLine($"Length of {sequence}: {sequence.Length}");
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

    string LookAndSay(string sequence, int iterations = 1)
    {
        while (iterations > 0)
        {
            var currentLook = sequence[0];
            var nextSequence = string.Empty;
            var currentSay = 1;
            for (var ix = 1; ix < sequence.Length; ix++)
            {
                if (currentLook == sequence[ix])
                {
                    currentSay++;
                }
                else
                {
                    nextSequence += $"{currentSay}{currentLook}";
                    currentSay = 1;
                }
                currentLook = sequence[ix];
            }
            nextSequence += $"{currentSay}{currentLook}";
            sequence = nextSequence;
            iterations--;
        }

        return sequence;
    }
}
