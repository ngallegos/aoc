using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2022.Modules;

public class Day25 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var sampleValues = get_sample()
            .Select(x => new { snafu = x, base10 = SNAFU.SNAFUToDecimal(x) })
            .ToList();
        var sampleSum = sampleValues.Sum(x => x.base10);
        
        var actualValues = get_input()
            .Select(x => new { snafu = x, base10 = SNAFU.SNAFUToDecimal(x) })
            .ToList();
        var actualSum = actualValues.Sum(x => x.base10);

        // foreach (var sv in sampleValues)
        // {
        //     Console.WriteLine($"{sv.snafu}\t{sv.base10}\t {SNAFU.DecimalToSNAFU(sv.base10)}");
        // }
        
        return new
        {
            sampleSum,
            sampleSumSnafu = SNAFU.DecimalToSNAFU(sampleSum),
            actualSum,
            actualSumSnafu = SNAFU.DecimalToSNAFU(actualSum)
        };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class SNAFU
    {
        private static List<(char snafuChar, long decimalValue)> _map = new List<(char snafuChar, long decimalValue)>
        {
            ('2', 2),
            ('1', 1),
            ('0', 0),
            ('-', -1),
            ('=', -2),
        };

        private static long _snafuBase = 5L;

        /// <summary>
        /// I was trying to do this backwards - starting with the highest digits
        /// Got help from <see href="https://www.reddit.com/r/adventofcode/comments/zur1an/comment/j1o63ly/?utm_source=share&utm_medium=web2x&context=3">this paste</see>
        /// on the <see href="https://www.reddit.com/r/adventofcode/comments/zur1an/2022_day_25_solutions/">solutions megathread</see>
        /// </summary>
        public static string DecimalToSNAFU(long value)
        {
            if (value == 0) return "0";
            var sb = new StringBuilder();

            while (value != 0)
            {
                var remainder = value % _snafuBase;
                value /= 5;

                if (remainder > 2)
                {
                    value++;
                    remainder -= 5;
                }

                var c = _map.First(x => x.decimalValue == remainder);
                sb.Insert(0, c.snafuChar);
            }

            return sb.ToString();
        }

        public static long SNAFUToDecimal(string snafu)
        {
            var snafuChars = snafu.ToCharArray().Reverse().ToArray();
            var decimalValue = 0L;
            for (int i = 0; i < snafuChars.Length; i++)
            {
                var snafuVal = _map.First(x => x.snafuChar == snafuChars[i]).decimalValue;
                var exp = (long)Math.Pow(_snafuBase, i);
                decimalValue += snafuVal * exp;
            }

            return decimalValue;
        }
    }
}