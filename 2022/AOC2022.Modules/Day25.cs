using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day25 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var sampleValues = get_sample()
            .Select(x => new { snafu = x, base10 = SNAFU.SNAFUToDecimal(x) })
            .ToList();
        var sampleSum = sampleValues.Sum(x => x.base10);

        foreach (var sv in sampleValues)
        {
            Console.WriteLine($"{sv.snafu}\t{sv.base10}\t {SNAFU.DecimalToSNAFU(sv.base10)}");
        }
        
        return new
        {
            sampleSum,
            sampleSumSnafu = SNAFU.DecimalToSNAFU(sampleSum)
        };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class SNAFU
    {
        private static List<(char snafuChar, int decimalValue)> _map = new List<(char snafuChar, int decimalValue)>
        {
            ('2', 2),
            ('1', 1),
            ('0', 0),
            ('-', -1),
            ('=', -2),
        };

        private static int _snafuBase = 5;

        public static string DecimalToSNAFU(int value)
        {
            var quotient = 0;
            var pow = 0;
            do
            {
                quotient = (int)value / ((int)Math.Pow(_snafuBase, pow));

                pow++;
            } while (quotient != 0);
           
            pow--;
            for (int i = pow; i <= 0; i--)
            {
                // TODO
            }
            Console.WriteLine($"Highest power of {_snafuBase}: {pow}");
            return null;
        }

        public static int SNAFUToDecimal(string snafu)
        {
            var snafuChars = snafu.ToCharArray().Reverse().ToArray();
            var decimalValue = 0;
            for (int i = 0; i < snafuChars.Length; i++)
            {
                var snafuVal = _map.First(x => x.snafuChar == snafuChars[i]).decimalValue;
                var exp = (int)Math.Pow(_snafuBase, i);
                decimalValue += snafuVal * exp;
            }

            return decimalValue;
        }
    }
}