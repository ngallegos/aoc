using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace AOC2021.Modules
{
    public class Day03 : DayBase
    {
        public override dynamic Part1()
        {
            var input = get_input();
            var inputs = input.Select(x => x.ToArray().Select(c => double.Parse(c.ToString())));
            var matrix = Matrix<double>.Build.DenseOfRows(inputs);
            var gammaBits = new List<int>();
            var epsilonBits = new List<int>();
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                var mostCommon = matrix.Column(i)
                    .AsArray()
                    .GroupBy(x => x)
                    .OrderByDescending(g => g.Count())
                    .Select(x => (int)x.Key).First();
                gammaBits.Add(mostCommon);
                epsilonBits.Add((mostCommon + 1) % 2);
            }

            var gammaRate = Convert.ToInt32(string.Join("", gammaBits.Select(x => x.ToString())), 2);
            var epsilonRate = Convert.ToInt32(string.Join("", epsilonBits.Select(x => x.ToString())), 2);

            return new { gammaRate, epsilonRate, powerConsumption = gammaRate * epsilonRate };
        }

        public override dynamic Part2()
        {
            return "NOT IMPLEMENTED";
        }
    }
}