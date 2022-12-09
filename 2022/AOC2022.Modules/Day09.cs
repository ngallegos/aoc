using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AOC2022.Modules;

public class Day09 : DayBase
{
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

    private readonly List<(string direction, int distance)> _instructions;

    public Day09()
    {
        _instructions = get_input<(string direction, int distance)>(s =>
        {
            var parts = s.Split(' ');
            return (parts[0], int.Parse(parts[1]));
        }, "Part1").ToList();
    }

    public override dynamic Part1()
    {
        var ropeState = new RopeSimulation();
        foreach (var instruction in _instructions)
        {
            ropeState.ExecuteInstruction(instruction.direction, instruction.distance);
        }

        return ropeState;
    }

    public override dynamic Part2()
    {
        var ropeState = new RopeSimulation(10);
        foreach (var instruction in _instructions)
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
                var leaderDirection = direction;
                for (int k = 0; k < _knots.Count - 1; k++)
                {
                    leaderDirection = FollowLeader(_knots[k], _knots[k + 1], leaderDirection);
                }
                _tailLocations.Add(Tail.ToString());
            }
        }
        
        
        private string FollowLeader(KnotPoint leader, KnotPoint follower, string leaderDirection)
        {
            if (!leader.IsTouching(follower))
            {
                follower.Move(leaderDirection);
                if (leader.XCoordinate != follower.XCoordinate
                    && leader.YCoordinate != follower.YCoordinate)
                {
                    var additionalDirection = "";
                    switch (leaderDirection)
                    {
                        case "U":
                        case "D":
                            additionalDirection = leader.XCoordinate > follower.XCoordinate ? "R" : "L";
                            break;
                        case "L":
                        case "R":
                            additionalDirection = leader.YCoordinate > follower.YCoordinate ? "U" : "D";
                            break;
                    }
                    follower.Move(additionalDirection);
                    return additionalDirection;
                }
            }

            return leaderDirection;
        }
        
        public void ExecuteInstruction(string direction, int distance, int knotIndex)
        {
            if (knotIndex == _knots.Count - 1)
                return;
            var leader = _knots[knotIndex];
            var follower = _knots[knotIndex + 1];
            for (int i = 0; i < distance; i++)
            {
                leader.Move(direction);
                if (!leader.IsTouching(follower))
                {
                    follower.Move(direction);
                    if (leader.XCoordinate != follower.XCoordinate
                        && leader.YCoordinate != follower.YCoordinate)
                    {
                        var additionalDirection = "";
                        switch (direction)
                        {
                            case "U":
                            case "D":
                                additionalDirection = leader.XCoordinate > follower.XCoordinate ? "R" : "L";
                                break;
                            case "L":
                            case "R":
                                additionalDirection = leader.YCoordinate > follower.YCoordinate ? "U" : "D";
                                break;
                        }
                        follower.Move(additionalDirection);
                    }
                    _tailLocations.Add(follower.ToString());
                }
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

            public string Move(string direction)
            {
                switch (direction)
                {
                    case "R":
                        XCoordinate++;
                        break;
                    case "L":
                        XCoordinate--;
                        break;
                    case "U":
                        YCoordinate++;
                        break;
                    case "D":
                        YCoordinate--;
                        break;
                }

                return direction;
            }

            public override string ToString()
            {
                return $"({XCoordinate},{YCoordinate})";
            }

            public override bool Equals(object obj)
            {
                if (obj is string)
                {
                    var other = new KnotPoint((string)obj);
                    return this.Equals(other);
                }
                return this.Equals(obj as KnotPoint);
            }

            protected bool Equals(KnotPoint other)
            {
                if (other == null)
                    return false;
                return XCoordinate == other.XCoordinate && YCoordinate == other.YCoordinate;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(XCoordinate, YCoordinate);
            }

            public static bool operator ==(KnotPoint a, KnotPoint b) => a?.Equals(b) ?? false;

            public static bool operator !=(KnotPoint a, KnotPoint b) => !(a == b);
        }
    }
}