namespace AOC2015.Tests;

public class Day01Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var instructions = get_sample().First();

        // Act
        var floor = FinalFloor(instructions);

        // Assert
        floor.ShouldBe(-3);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var instructions = get_input().First();

        // Act
        var floor = FinalFloor(instructions);

        // Assert
        floor.ShouldBe(74);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var instructions = get_sample(2).First();

        // Act
        var position = FirstBasementPosition(instructions);

        // Assert
        position.ShouldBe(5);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var instructions = get_input().First();

        // Act
        var position = FirstBasementPosition(instructions);

        // Assert
        position.ShouldBe(1795);
    }

    private static int FloorChange(char instruction) => instruction switch
    {
        '(' => 1,
        ')' => -1,
        _ => 0
    };

    private static int FinalFloor(string instructions) => instructions.Sum(FloorChange);

    private static int FirstBasementPosition(string instructions)
    {
        var floor = 0;
        for (var i = 0; i < instructions.Length; i++)
        {
            floor += FloorChange(instructions[i]);
            if (floor < 0)
                return i + 1;
        }

        return 0;
    }
}
