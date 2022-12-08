using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using AOC2022.Modules;
using Newtonsoft.Json;

namespace AOC2022.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseDayType = typeof(DayBase);
            var days = typeof(DayBase).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(baseDayType) && !x.IsAbstract)
                .OrderBy(x => x.Name);

            foreach (var dayType in days)
            {
                var dayNumber = int.Parse(dayType.Name.Replace("Day", ""));
                System.Console.WriteLine($"-----DAY {dayNumber:00}---------------------------------------------\n");
                var day = Activator.CreateInstance(dayType) as DayBase;
                var part1Results = day.Part1();
                System.Console.WriteLine($"PART 01:\n{JsonConvert.SerializeObject(part1Results, Formatting.Indented)}");
                var part2Results = day.Part2();
                System.Console.WriteLine($"PART 02:\n{JsonConvert.SerializeObject(part2Results, Formatting.Indented)}");
                System.Console.WriteLine();
            }
            
        }
    }
}