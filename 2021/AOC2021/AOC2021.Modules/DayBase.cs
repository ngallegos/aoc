using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOC2021.Modules
{
    public abstract class DayBase
    {
        
        protected IEnumerable<string> get_input([CallerMemberName] string callerName = "")
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var dayType = this.GetType();
            var dayNumber = int.Parse(dayType.Name.Replace("Day", ""));
            var partNumber = int.Parse(callerName.Replace("Part", ""));
            var resources = assembly.GetManifestResourceNames();
            var resouceName = resources.FirstOrDefault(x => x.EndsWith($"day-{dayNumber:00}-{partNumber:00}.txt")
                                                            || x.EndsWith($"day-{dayNumber:00}-01.txt"));
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

        public abstract dynamic Part1();
        public abstract dynamic Part2();

    }
}