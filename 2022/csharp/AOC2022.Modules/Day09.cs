using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AOC2022.Modules;

public class Day09 : DayBase
{
    public override bool Ignore => true;
    private readonly List<(string direction, int distance)> _sample = new List<string>
    {
        "R 4",
        "U 4",
        "L 3",
        "D 1",
        "R 4",
        "D 1",
        "L 5",
        "R 2"
    }.Select(s => { 
        var parts = s.Split(' ');
        var (direction, distance) = (parts[0], int.Parse(parts[1]));
        return (direction, distance);
    }).ToList();
    
    
    private readonly List<(string direction, int distance)> _sample2 = new List<string>
    {
        "R 5",
        "U 8",
        "L 8",
        "D 3",
        "R 17",
        "D 10",
        "L 25",
        "U 20"
    }.Select(s => { 
        var parts = s.Split(' ');
        var (direction, distance) = (parts[0], int.Parse(parts[1]));
        return (direction, distance);
    }).ToList();

    public override dynamic Part1()
    {
        var instructions = get_input<(string direction, int distance)>(s =>
            {
                var parts = s.Split(' ');
                return (parts[0], int.Parse(parts[1]));
            }).ToList();
        var ropeState = new RopeSimulation();
        foreach (var instruction in instructions)
        {
            ropeState.ExecuteInstruction(instruction.direction, instruction.distance);
        }

        return ropeState;
    }

    public override dynamic Part2()
    {
        var instructions = get_input<(string direction, int distance)>(s =>
        {
            var parts = s.Split(' ');
            return (parts[0], int.Parse(parts[1]));
        }).ToList();
        var ropeState = new RopeSimulation(10);
        foreach (var instruction in instructions)
        {
            ropeState.ExecuteInstruction(instruction.direction, instruction.distance);
        }

        return ropeState;
    }

    private class RopeSimulation
    {
        private List<KnotPoint> _knots;

        private KnotPoint Head => _knots.First();

        private KnotPoint Tail => _knots.Last();

        private List<string> _tailLocations;

        public int UniqueTailLocationsCount => _tailLocations.Distinct().Count();
        
        public RopeSimulation(int knotCount = 2)
        {
            _knots = Enumerable.Range(1, knotCount).Select(x => new KnotPoint(0, 0)).ToList();
            _tailLocations = new List<string>
            {
                _knots.Last().ToString()
            };
        }

        public void ExecuteInstruction(string direction, int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                Head.Move(direction);
                for (int k = 0; k < _knots.Count - 1; k++)
                {
                    FollowLeader(_knots[k], _knots[k + 1]);
                    _tailLocations.Add(Tail.ToString());
                }
            }
        }
        
        
        private void FollowLeader(KnotPoint leader, KnotPoint follower)
        {
            if (!leader.IsTouching(follower))
            {
                var direction = "";
                if (leader.XCoordinate > follower.XCoordinate)
                    direction += "R";
                else if (leader.XCoordinate < follower.XCoordinate)
                    direction += "L";

                if (leader.YCoordinate > follower.YCoordinate)
                    direction += "U";
                else if (leader.YCoordinate < follower.YCoordinate)
                    direction += "D";
                
                follower.Move(direction);
            }
        }
        

        private class KnotPoint
        {
            public KnotPoint(int x, int y)
            {
                XCoordinate = x;
                YCoordinate = y;
            }
            public KnotPoint(string input)
            {
                input = input.TrimStart('(').TrimEnd(')');
                var coordinates = input.Split(',');
                XCoordinate = int.Parse(coordinates[0]);
                YCoordinate = int.Parse(coordinates[1]);
            }
            public int XCoordinate { get; private set; }
            public int YCoordinate { get; private set; }

            public bool IsTouching(KnotPoint other)
            {
                var xRange = Enumerable.Range(other.XCoordinate - 1, 3);
                var yRange = Enumerable.Range(other.YCoordinate - 1, 3);

                return xRange.Contains(XCoordinate) && yRange.Contains(YCoordinate);
            }

            public void Move(string direction)
            {
                foreach (var step in direction)
                {
                    switch (step)
                    {
                        case 'R':
                            XCoordinate++;
                            break;
                        case 'L':
                            XCoordinate--;
                            break;
                        case 'U':
                            YCoordinate++;
                            break;
                        case 'D':
                            YCoordinate--;
                            break;
                    }
                }
            }

            public override string ToString()
            {
                return $"({XCoordinate},{YCoordinate})";
            }
        }
    }
}