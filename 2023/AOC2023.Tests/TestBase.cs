using System.Diagnostics;
using System.Reflection;

namespace AOC2023.Tests;

public abstract class TestBase
{
    [Test]
    public virtual void Part1_Sample()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    public virtual void Part1_Actual()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    public virtual void Part2_Sample()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    public virtual void Part2_Actual()
    {
        throw new NotImplementedException();
    }

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
        var inputs = get_input(partNumber, type);
        var t = new Stopwatch();
        t.Start();
        foreach (var input in inputs)
        {
            yield return transform(input);
        }
        t.Stop();
        Console.WriteLine($"Input transform:\t{t.ElapsedMilliseconds}ms");
    }
    
    protected IEnumerable<T> get_sample<T>(Func<string, T> transform, int partNumber = 1, string type = "Input")
    {
        var inputs = get_sample(partNumber);
        var t = new Stopwatch();
        t.Start();
        foreach (var input in inputs)
        {
            yield return transform(input);
        }
        t.Stop();
        Console.WriteLine($"Input transform:\t{t.ElapsedMilliseconds}ms");
    }

    protected T[][] get_sample_grid<T>(Func<char, T> transform, int partNumber = 1)
    {
        var inputs = get_sample(partNumber).ToList();
        var grid = new T[inputs.Count][];
        for (var i = 0; i < inputs.Count; i++)
        {
            grid[i] = new T[inputs[i].Length];
            for (var j = 0; j < inputs[i].Length; j++)
            {
                grid[i][j] = transform(inputs[i][j]);
            }
        }

        return grid;
    }
}