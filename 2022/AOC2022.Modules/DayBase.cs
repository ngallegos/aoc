using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOC2022.Modules
{
    public abstract class DayBase
    {
        public abstract bool Completed { get; }

        protected IEnumerable<string> get_sample([CallerMemberName] string callerName = "Part1")
        {
            return get_input(callerName, "Sample");
        }
        
        protected IEnumerable<T> get_sample<T>(Func<string, T> transform, [CallerMemberName] string callerName = "Part1")
        {
            return get_input(transform, callerName, "Sample");
        }

        protected IEnumerable<string> get_input([CallerMemberName] string callerName = "Part1", string type = "Input")
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var dayType = this.GetType();
            var dayNumber = int.Parse(dayType.Name.Replace("Day", ""));
            var partNumber = int.Parse(callerName.Replace("Part", ""));
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
        }
        
        protected IEnumerable<T> get_input<T>(Func<string, T> transform, [CallerMemberName] string callerName = "Part1", string type = "Input")
        {
            var inputs = get_input(callerName, type);
            foreach (var input in inputs)
            {
                yield return transform(input);
            }
        }

        public abstract dynamic Part1();
        public abstract dynamic Part2();

    }
}