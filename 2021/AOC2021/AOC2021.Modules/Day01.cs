using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2021.Modules
{
    public class Day01
    {
        
        private IEnumerable<string> get_input(){
            
            var assembly = this.GetType().GetTypeInfo().Assembly;
            using (var s = assembly.GetManifestResourceStream("AOC2021.Modules.Inputs.day-01-01.txt"))
            {
                using (var sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        yield return line;
                }
            }
        }
        public (int increased, int decreased) Part1()
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

            return (increased, decreased);
        }

        public (int increased, int decreased) Part2()
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

            return (increased, decreased);
        }
    }
}