using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AOC2023.Modules;
using Newtonsoft.Json;

namespace AOC2023.Console
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
                if (day.Ignore)
                    continue;
                System.Console.WriteLine($"-----DAY {dayNumber:00}---------------------------------------------\n");
                var s = new Stopwatch();
                s.Start();
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
                s.Stop();
                var p1Elapsed = s.ElapsedMilliseconds;
                System.Console.WriteLine($"Part 01:\t{p1Elapsed}ms");
                
                s.Restart();

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
                s.Stop();
                var p2Elapsed = s.ElapsedMilliseconds;
                System.Console.WriteLine($"Part 02:\t{p2Elapsed}ms");
                System.Console.WriteLine($"Total:\t\t{p1Elapsed + p2Elapsed}ms");

                System.Console.WriteLine();
            }
        }
    }
}