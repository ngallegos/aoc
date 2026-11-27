namespace AOC2015.Tests;

public class Day02Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var presents = get_sample().Select(x => new Present(x)).ToList();

        // Act
        var total = presents.Sum(p => p.SquareFeetOfWrappingPaperNeeded);

        // Assert
        total.ShouldBe(101);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var presents = get_input().Select(x => new Present(x)).ToList();

        // Act
        var total = presents.Sum(p => p.SquareFeetOfWrappingPaperNeeded);

        // Assert
        total.ShouldBe(1588178);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var presents = get_sample().Select(x => new Present(x)).ToList();

        // Act
        var total = presents.Sum(p => p.FeetOfRibbonNeeded);

        // Assert
        total.ShouldBe(48);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var presents = get_input().Select(x => new Present(x)).ToList();

        // Act
        var total = presents.Sum(p => p.FeetOfRibbonNeeded);

        // Assert
        total.ShouldBe(3783758);
    }

    
    private class Present
    {
        private int[] _dimensions;
        private int _squareFeetOfWrappingPaperNeeded;
        private int _feetOfRibbonNeeded;

        public Present(string dimensions)
        {
            _dimensions = dimensions.Split('x').Select(int.Parse)
                .OrderBy(x => x).ToArray();
            _squareFeetOfWrappingPaperNeeded = 2 * (_dimensions[0] * _dimensions[1] 
                                                    + _dimensions[1] * _dimensions[2] 
                                                    + _dimensions[0] * _dimensions[2])
                                               + _dimensions[0] * _dimensions[1];
            _feetOfRibbonNeeded = 2 * (_dimensions[0] + _dimensions[1])
                                  + _dimensions.Aggregate((x, y) => x * y);
        }

        public int SquareFeetOfWrappingPaperNeeded => _squareFeetOfWrappingPaperNeeded;
        public int FeetOfRibbonNeeded => _feetOfRibbonNeeded;
    }
}
