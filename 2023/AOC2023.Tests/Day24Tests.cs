using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day24Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var hail = get_sample(x => new Hailstone(x))
            .ToList();
        var futureIntersections = GetFutureXYIntersections(hail, (7, 27));
        futureIntersections.ShouldBe(2);
    }

    protected override void SolvePart1_Actual()
    {
        var hail = get_input(x => new Hailstone(x))
            .ToList();
        var futureIntersections = GetFutureXYIntersections(hail, (200000000000000, 400000000000000));
        futureIntersections.ShouldBe(2);
    }

    private int GetFutureXYIntersections(List<Hailstone> hail, (long x, long y) bounds)
    {
        var futureIntersections = 0;
        for (int i = 0; i < hail.Count; i++)
        {
            var hailstone = hail[i];
            foreach (var other in hail.Skip(i+1))
            {
                var (intersects, intersection) = hailstone.IntersectsXY(other, bounds);
                if (intersects && intersection.HasValue)
                {
                    var t = hailstone.GetTimeAtX(intersection.Value.x);
                    if (t > 0) // future
                        futureIntersections++;
                }
            }
        }

        return futureIntersections;
    }
    
    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private class Hailstone
    {
        private static Regex _hailstoneRegex = new Regex(@"^(?<px>\d+), (?<py>\d+), (?<pz>\d+) @[ ]+(?<vx>-?\d+),[ ]+(?<vy>-?\d+),[ ]+(?<vz>-?\d+)$");
        public (decimal x, decimal y, decimal z) Position { get; private set; }
        public (int x, int y, int z) Velocity { get; private set; }
        private decimal _m;
        private decimal _b;
        public decimal Slope => _m;
        public decimal Intercept => _b;
        public string Definition { get; private set; }

        public Hailstone(string definition)
        {
            Definition = definition;
            var match = _hailstoneRegex.Match(definition);
            if (!match.Success)
                throw new ArgumentException($"Invalid definition {definition}", nameof(definition));
            Position = (decimal.Parse(match.Groups["px"].Value), decimal.Parse(match.Groups["py"].Value), decimal.Parse(match.Groups["pz"].Value));
            Velocity = (int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value), int.Parse(match.Groups["vz"].Value));
            var nextPosition = GetPosition(1);
            _m = (Position.y - nextPosition.y) / (Position.x - nextPosition.x);
            _b = Position.y - _m * Position.x;
        }
        
        private (decimal x, decimal y, decimal z) GetPosition(long time = 0)
        {
            return (Position.x + Velocity.x * time, Position.y + Velocity.y * time, Position.z + Velocity.z * time);
        }

        private decimal Y(decimal x)
        {
            return _m * x + _b;
        }
        
        private (decimal x, decimal y)? FindXYIntersection(Hailstone other)
        {
            // (m1x + b1) = (m2x + b2)
            // m1x - m2x = b2 - b1
            // x(m1 - m2) = b2 - b1
            // x = (b2 - b1) / (m1 - m2)
            if (Math.Abs(Slope) == Math.Abs(other.Slope))
                return null;
            var x = (other.Intercept - Intercept) / (Slope - other.Slope); 
            return (x, Y(x));
        }
        
        public long GetTimeAtX(decimal x)
        {
            return (long)Math.Floor((x - Position.x) / Velocity.x);
        }

        public (bool, (decimal x, decimal y)?) IntersectsXY(Hailstone other, (long min, long max) bounds)
        {
            var isect = FindXYIntersection(other);
            if (!isect.HasValue)
                return (false, null);
            var intersection = isect.Value;
            if (intersection.x < bounds.min 
                || intersection.x > bounds.max
                || intersection.y < bounds.min
                || intersection.y > bounds.max)
                return (false, null);
            return (true, intersection);
        }
        
        
        public override string ToString()
        {
            return $"y = {_m}x + {_b}";
        }
    }
}