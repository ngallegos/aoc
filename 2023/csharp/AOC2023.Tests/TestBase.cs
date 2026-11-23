using System.Diagnostics;
using System.Reflection;

namespace AOC2023.Tests;

public abstract class TestBase
{
    protected abstract void SolvePart1_Sample();
    protected abstract void SolvePart1_Actual();
    protected abstract void SolvePart2_Sample();
    protected abstract void SolvePart2_Actual();
    
    [Test]
    public void Part1_Sample() => Execute(SolvePart1_Sample, "Part 1 sample");

    [Test]
    public void Part1_Actual() => Execute(SolvePart1_Actual, "Part 1 actual");

    [Test]
    public void Part2_Sample() => Execute(SolvePart2_Sample, "Part 2 sample");

    [Test]
    public void Part2_Actual() => Execute(SolvePart2_Actual, "Part 2 actual");

    private void Execute(Action solve, string label)
    {
        var t = Stopwatch.StartNew();
        try
        {
            solve();
        }
        catch (NotImplementedException)
        {
            Assert.Ignore($"{label} is not implemented yet");
        }
        finally
        {
            t.Stop();
            Console.WriteLine($"{label}:\t{t.ElapsedMilliseconds}ms");
        }
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
        
    protected IEnumerable<T> get_input<T>(Func<string, T> transform, int partNumber = 1, string type = "day")
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
    
    protected IEnumerable<T> get_sample<T>(Func<string, T> transform, int partNumber = 1, string type = "day")
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

    protected T[,] get_sample_grid<T>(Func<char, (int x, int y), T> transform, int partNumber = 1)
    {
        var inputs = get_sample(partNumber).ToList();
        var grid = new T[inputs[0].Length, inputs.Count];
        for (var j = 0; j < inputs.Count; j++)
        {
            for (var i = 0; i < inputs[j].Length; i++)
            {
                grid[i,j] = transform(inputs[j][i], (i, j));
            }
        }

        return grid;
    }
    
    protected T[,] get_input_grid<T>(Func<char, (int x, int y), T> transform, int partNumber = 1)
    {
        var inputs = get_input(partNumber).ToList();
        var grid = new T[inputs[0].Length, inputs.Count];
        for (var j = 0; j < inputs.Count; j++)
        {
            for (var i = 0; i < inputs[j].Length; i++)
            {
                grid[i,j] = transform(inputs[j][i], (i, j));
            }
        }

        return grid;
    }
}