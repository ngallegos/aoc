using System.Collections.Generic;
using System.Linq;

namespace AOC2015.Modules;

public class Day02 : DayBase
{
    public override bool Completed { get; }
    private readonly List<Present> _presents;

    public Day02()
    {
        _presents = get_input("Part1").Select(x => new Present(x)).ToList();
    }
    
    public override dynamic Part1()
    {
        return new
        {
            squareFeetOfWrappingPaperNeeded = _presents.Sum(p => p.SquareFeetOfWrappingPaperNeeded),
            feetOfRibbonNeeded = _presents.Sum(p => p.FeetOfRibbonNeeded )
        };
    }

    public override dynamic Part2()
    {
        return new
        {
            squareFeetOfWrappingPaperNeeded = _presents.Sum(p => p.SquareFeetOfWrappingPaperNeeded),
            feetOfRibbonNeeded = _presents.Sum(p => p.FeetOfRibbonNeeded )
        };
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