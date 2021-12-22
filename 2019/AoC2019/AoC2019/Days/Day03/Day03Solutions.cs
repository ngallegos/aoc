using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AoC2019.Days.Day03
{
    public class Day03Solutions : DaySolutionsBase<Day03Inputs>
    {
        public override int Day => 3;
        protected override string SolvePuzzle01(List<string> input)
        {
            var wire01Path = InterpretWirePath(input[0].Split(','));
            var wire02Path = InterpretWirePath(input[1].Split(','));

            var intersectingPoints = new List<Point>();

            for (int i = 1; i < wire01Path.Count; i++)
            {
                for(int j = 1; j < wire02Path.Count; j++)
                {

                    var intersection = LineLineIntersection(wire01Path[i - 1], wire01Path[i], wire02Path[j - 1],
                        wire02Path[j]);

                    if (intersection.X != 0 || intersection.Y != 0)
                        intersectingPoints.Add(intersection);
                }
            }

            intersectingPoints = intersectingPoints.Distinct().ToList();

            var closestIntersectionManhattanDistance = intersectingPoints.Distinct().Min(CalculateManhattanDistanceFromOrigin);

            return closestIntersectionManhattanDistance.ToString();
        }

        protected override string SolvePuzzle02(List<string> input)
        {
            throw new NotImplementedException();
        }

        // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
        public static Point LineLineIntersection(Point A, Point B, Point C, Point D)
        {

            int denom = ((A.X - B.X) * (C.Y - D.Y) - (A.Y - B.Y) * (C.X - D.X));
            if (denom == 0)
                return new Point(0,0);

            int x = ((A.X*B.Y - A.Y*B.X)*(C.X - D.X) - (A.X - B.X)*(C.X*D.Y - C.Y*D.X))/denom;
            var y = ((A.X * B.Y - A.Y * B.X) * (C.Y - D.Y) - (A.Y - B.Y) * (C.X * D.Y - C.Y * D.X)) / denom;

            var isect = new Point(x, y);

            bool isInSegments = true;

            if (A.X == B.X)
                isInSegments = isInSegments && A.X == x;
            else if (A.Y == B.Y)
                isInSegments = isInSegments && A.Y == y;



            return isect;

            double dt = ((A.X - C.X)*(C.Y - D.Y) - (A.Y - C.Y)*(C.X - D.X))/denom;
            double du = ((A.X - B.X)*(A.Y - C.Y) - (A.Y - B.Y)*(A.X - C.X))/denom;

            int t = (int)Math.Round(dt);
            int u = (int) Math.Round(du);

            if (t < 0 || t > 1 || u < 0 || u > 1)
                return new Point(0,0);

            var isectT = new Point(A.X + t * (B.X - A.X), A.Y + t * (B.Y - A.Y));

            var isectU = new Point(C.X + u*(D.X - C.X), C.Y + u*(D.Y - C.Y));

            if (isectU == isectT)
                return isectT;

            return new Point(0,0);
        }

        private List<Point> InterpretWirePath(IEnumerable<string> instructions)
        {
            var points = new List<Point>();
            int x = 0, y = 0;
            points.Add(new Point(x, y));
            foreach (var instr in instructions)
            {
                var direction = instr[0];
                var distance = Int32.Parse(instr.Substring(1));
                switch (direction)
                {
                    case 'U':
                        y += distance;
                        break;
                    case 'D':
                        y -= distance;
                        break;
                    case 'L':
                        x -= distance;
                        break;
                    case 'R':
                        x += distance;
                        break;
                }
                //Console.Write($"({x},{y}) ");
                points.Add(new Point(x, y));
            }

            //
            //Console.WriteLine();
            return points;
        }

        private int CalculateManhattanDistanceFromOrigin(Point p)
        {
            return Math.Abs(p.X) + Math.Abs(p.Y);
        }
    }
}
