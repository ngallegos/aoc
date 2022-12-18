using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day18 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var sampleDroplet = new LavaDroplet(get_sample());
        var actualDroplet = new LavaDroplet(get_input());

        sampleDroplet.Analyze();
        actualDroplet.Analyze();
        
        return new
        {
            sampleDroplet,
            actualDroplet
        };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class LavaDroplet
    {
        private List<Cube> _cubes;

        public int SurfaceArea => _cubes.Sum(c => c.ExposedSides);

        public LavaDroplet(IEnumerable<string> input)
        {
            _cubes = input.Select(x => new Cube(x))
                .ToList();;
        }

        public void Analyze()
        {
            _cubes.ForEach(x => x.FindConnections(_cubes.Where(c => c != x && !c.Analyzed)));
        }
    }
    
    private class Cube
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        
        public bool Analyzed { get; private set; }

        public List<Cube> XNeighbors { get; private set; } = new List<Cube>();
        public List<Cube> YNeighbors { get; private set; } = new List<Cube>();
        public List<Cube> ZNeighbors { get; private set; } = new List<Cube>();

        public int ExposedSides => 6 - XNeighbors.Count - YNeighbors.Count - ZNeighbors.Count;

        public Cube(string input)
        {
            var coordinates = input.Split(',');
            X = int.Parse(coordinates[0]);
            Y = int.Parse(coordinates[1]);
            Z = int.Parse(coordinates[2]);
        }
        
        public void FindConnections(IEnumerable<Cube> cubes)
        {
            foreach (var cube in cubes)
            {
                if (cube.Analyzed)
                    continue;
                if ((cube.X == X - 1 || cube.X == X + 1) && cube.Y == Y && cube.Z == Z)
                {
                    XNeighbors.Add(cube);
                    cube.XNeighbors.Add(this);
                }
                if ((cube.Y == Y - 1 || cube.Y == Y + 1) && cube.X == X && cube.Z == Z)
                {
                    YNeighbors.Add(cube);
                    cube.YNeighbors.Add(this);
                }
                if ((cube.Z == Z - 1 || cube.Z == Z + 1) && cube.Y == Y && cube.X == X)
                {
                    ZNeighbors.Add(cube);
                    cube.ZNeighbors.Add(this);
                }
            }

            Analyzed = true;
        }
        
    }
}