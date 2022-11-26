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
                System.Console.WriteLine($"\tPART 01: {JsonConvert.SerializeObject(part1Results)}");
                var part2Results = day.Part2();
                System.Console.WriteLine($"\tPART 02: {JsonConvert.SerializeObject(part2Results)}");
                System.Console.WriteLine();
            }
            
        }
    }
}