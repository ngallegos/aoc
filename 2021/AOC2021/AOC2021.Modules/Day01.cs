using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2021.Modules
{
    public class Day01 : DayBase
    {
        public override dynamic Part1()
        {
            var increased = 0;
            var decreased = 0;
            var readings = get_input().Select(int.Parse).ToArray();
            for(int i = 1; i < readings.Length; i++)
            {
                if (readings[i] > readings[i - 1])
                    increased++;
                else if (readings[i] < readings[i - 1])
                    decreased++;
            }

            return new { increased,  decreased };
        }

        public override dynamic Part2()
        {
            
            var increased = 0;
            var decreased = 0;
            var readings = get_input().Select(int.Parse).ToArray();
            for(int i = 3; i < readings.Length; i++)
            {
                var sumCurrent = readings[i] + readings[i - 1] + readings[i - 2];
                var sumPrev = readings[i - 1] + readings[i - 2] + readings[i - 3];
                if (sumCurrent > sumPrev)
                    increased++;
                else if (sumCurrent < sumPrev)
                    decreased++;
            }

            return new {increased,  decreased};
        }
    }
}