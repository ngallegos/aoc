using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace AOC2022.Modules
{
    public abstract class DayBase
    {
        public abstract bool Ignore { get; }

        protected IEnumerable<string> get_sample()
        {
            return get_input(type: "Sample");
        }

        protected IEnumerable<string> get_input(int partNumber = 1, string type = "Input")
        {
            var t = new Stopwatch();
            t.Start();
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var dayType = this.GetType();
            var dayNumber = int.Parse(dayType.Name.Replace("Day", ""));
            var resources = assembly.GetManifestResourceNames();
            var resouceName = resources.FirstOrDefault(x => x.Contains(type) && (x.EndsWith($"day-{dayNumber:00}-{partNumber:00}.txt")
                                                            || x.EndsWith($"day-{dayNumber:00}-01.txt")));
            using (var s = assembly.GetManifestResourceStream($"{resouceName}"))
            {
                using (var sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        yield return line;
                }
            }
            t.Stop();
            Console.WriteLine($"Input parsing:\t{t.ElapsedMilliseconds}ms");
        }
        
        protected IEnumerable<T> get_input<T>(Func<string, T> transform, int partNumber = 1, string type = "Input")
        {
            var inputs = get_input(1, type);
            var t = new Stopwatch();
            t.Start();
            foreach (var input in inputs)
            {
                yield return transform(input);
            }
            t.Stop();
            Console.WriteLine($"Input transform:\t{t.ElapsedMilliseconds}ms");
        }

        public abstract dynamic Part1();
        public abstract dynamic Part2();

        public virtual async Task<dynamic> Part1Async()
        {
            return await new ValueTask<dynamic>(Part1());
        }
        public virtual async Task<dynamic> Part2Async()
        {
            return await new ValueTask<dynamic>(Part2());
        }
    }
}