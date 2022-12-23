using System.Collections.Generic;
using System.Linq;
using AOC2022.Modules.Shared;

namespace AOC2022.Modules;

public class Day23 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private static (int x, int y)[] _neighbors = new[]
    {
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (-1, 0),
        (-1, -1),
        (-1, 0),
        (-1, 1),
    };
    
    private class Elf
    {
        public GridLocation Location { get; private set; }
        public GridLocation Proposed { get; private set; }

        public Elf(GridLocation initialLocation)
        {
            Location = initialLocation;
        }

        public GridLocation ConsiderNewLocation(List<Elf> elves)
        {
            var adjacent = _neighbors.Select(l => this.Location + l);
            
        }
    }
}