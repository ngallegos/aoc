using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AOC2022.Modules;
using Newtonsoft.Json;

namespace AOC2022.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var baseDayType = typeof(DayBase);
            var days = typeof(DayBase).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(baseDayType) && !x.IsAbstract)
                .OrderBy(x => x.Name);

            foreach (var dayType in days)
            {
                var dayNumber = int.Parse(dayType.Name.Replace("Day", ""));
                var day = Activator.CreateInstance(dayType) as DayBase;
                if (day.Completed)
                    continue;
                System.Console.WriteLine($"-----DAY {dayNumber:00}---------------------------------------------\n");
                try
                {
                    var part1Results = await day.Part1Async();
                    System.Console.WriteLine(
                        $"PART 01:\n{JsonConvert.SerializeObject(part1Results, Formatting.Indented)}");
                }
                catch (NotImplementedException)
                {
                    System.Console.WriteLine("PART 01:\nNot Implemented");
                }

                try
                {
                    var part2Results = await day.Part2Async();
                    System.Console.WriteLine(
                        $"PART 02:\n{JsonConvert.SerializeObject(part2Results, Formatting.Indented)}");
                }
                catch (NotImplementedException)
                {
                    System.Console.WriteLine("PART 02:\nNot Implemented");
                }

                System.Console.WriteLine();
            }
        }
    }
}