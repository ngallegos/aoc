using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace AOC2022.Modules
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

            var gammaRate = BinaryIntArrayToInt(gammaBits);
            var epsilonRate = BinaryIntArrayToInt(epsilonBits);

            return new { gammaRate, epsilonRate, powerConsumption = gammaRate * epsilonRate };
        }

        public override dynamic Part2()
        {
            var input = get_input();
            var inputs = input.Select(x => x.ToArray().Select(c => double.Parse(c.ToString())));
            var matrix = Matrix<double>.Build.DenseOfRows(inputs);
            var o2GenBits = new List<int>();
            var co2ScrubBits = new List<int>();
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                var col = matrix.Column(i).AsArray().ToList();
                var mostCommon = matrix.Column(i)
                    .AsArray()
                    .GroupBy(x => x)
                    .OrderByDescending(g => g.Count())
                    .Select(x => (int)x.Key).First();
                var leastCommon = (mostCommon + 1) % 2;
                o2GenBits.Add(mostCommon);
                co2ScrubBits.Add((mostCommon + 1) % 2);
                
                matrix.
            }

            var gammaRate = BinaryIntArrayToInt(o2GenBits);
            var epsilonRate = BinaryIntArrayToInt(co2ScrubBits);

            return new { gammaRate, epsilonRate, powerConsumption = gammaRate * epsilonRate };
        }

        private IEnumerable<IEnumerable<int>> findRatingsMatchingBitForColumn(IEnumerable<IEnumerable<int>> inputs, int bitVal)
        {
            
        }

        private int BinaryIntArrayToInt(IEnumerable<int> binaryString)
        {
            return Convert.ToInt32(string.Join("", binaryString.Select(x => x.ToString())), 2);
        }
    }
}