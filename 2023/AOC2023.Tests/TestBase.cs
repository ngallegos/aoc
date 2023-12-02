using System.Diagnostics;
using System.Reflection;

namespace AOC2023.Tests;

public class TestBase
{
    protected IEnumerable<string> get_sample(int partNumber = 1)
    {
        return get_input(partNumber: partNumber, type: "sample");
    }

    protected IEnumerable<string> get_input(int partNumber = 1, string type = "day")
    {
        var t = new Stopwatch();
        t.Start();
        var assembly = this.GetType().GetTypeInfo().Assembly;
        var dayType = this.GetType();
        var dayNumber = int.Parse(dayType.Name.Replace("Day", "").Replace("Tests", ""));
        var resources = assembly.GetManifestResourceNames();
        var resouceName = resources.FirstOrDefault(x => x.Contains(type) && x.EndsWith($"{type}-{dayNumber:00}-{partNumber:00}.txt"));
        
        resouceName ??= resources.FirstOrDefault(x => x.Contains(type) && x.EndsWith($"{type}-{dayNumber:00}-01.txt"));
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
}