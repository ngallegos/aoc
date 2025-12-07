global using Shouldly;

using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOC2025.Tests;

public abstract class TestBase
{
    protected abstract void SolvePart1_Sample();
    protected abstract void SolvePart1_Actual();
    protected abstract void SolvePart2_Sample();
    protected abstract void SolvePart2_Actual();

    string TestClassName => this.GetType().Name;
    
    [Test]
    public void Part1_Sample()
    {
        var t = Stopwatch.StartNew();
        try
        {
            if (IsIgnored(nameof(SolvePart1_Sample), out var reason))
            {
                Assert.Ignore($"{TestClassName} {reason ?? "Test is ignored"}");
                return;
            }
            SolvePart1_Sample();
        }
        finally
        {
            t.Stop();
            Console.WriteLine($"{TestClassName} Part 1 sample:\t{t.ElapsedMilliseconds}ms");            
        }

    }
    
    [Test]
    public void Part1_Actual()
    {
        var t = Stopwatch.StartNew();
        try
        {
            if (IsIgnored(nameof(SolvePart1_Actual), out var reason))
            {
                Assert.Ignore($"{TestClassName} {reason ?? "Test is ignored"}");
                return;
            }
            SolvePart1_Actual();
        }
        finally
        {
            t.Stop();
            Console.WriteLine($"{TestClassName} Part 1 actual:\t{t.ElapsedMilliseconds}ms");            
        }
    }
    
    [Test]
    public void Part2_Sample()
    {
        var t = Stopwatch.StartNew();try
        {
            if (IsIgnored(nameof(SolvePart2_Sample), out var reason))
            {
                Assert.Ignore($"{TestClassName} {reason ?? "Test is ignored"}");
                return;
            }
            SolvePart2_Sample();
        }
        finally
        {
            t.Stop();
            Console.WriteLine($"{TestClassName} Part 2 sample:\t{t.ElapsedMilliseconds}ms");            
        }
    }
    
    [Test]
    public void Part2_Actual()
    {
        var t = Stopwatch.StartNew();try
        {
            if (IsIgnored(nameof(SolvePart2_Actual), out var reason))
            {
                Assert.Ignore($"{TestClassName} {reason ?? "Test is ignored"}");
                return;
            }
            SolvePart2_Actual();
        }
        finally
        {
            t.Stop();
            Console.WriteLine($"{TestClassName} Part 2 actual:\t{t.ElapsedMilliseconds}ms");            
        }
    }

    bool IsIgnored(string methodName, out string? message)
    {
        var instanceType = this.GetType();
        var methodInfo = instanceType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        
        var ignoreAttribute = methodInfo?.GetCustomAttributes(typeof(IgnoreAttribute), false).FirstOrDefault() as IgnoreAttribute;
        
        message = ignoreAttribute?.Reason;
        
        return ignoreAttribute != null;
    }

    protected IEnumerable<string> get_sample(int partNumber = 1, [CallerMemberName] string callerName = "")
    {
        return get_input(partNumber: partNumber, type: "sample", callerName: callerName);
    }

    protected IEnumerable<string> get_input(int partNumber = 1, string type = "day", [CallerMemberName] string callerName = "")
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
            if (s != null)
            {
                using var sr = new StreamReader(s);
                string? line;
                while ((line = sr.ReadLine()) != null)
                    yield return line;
            }
        }
        t.Stop();
        Console.WriteLine($"{TestClassName}.{callerName} Input parsing:\t{t.ElapsedMilliseconds}ms");
    }
        
    protected IEnumerable<T> get_input<T>(Func<string, T> transform, int partNumber = 1, string type = "day", [CallerMemberName] string callerName = "")
    {
        var inputs = get_input(partNumber, type);
        var t = new Stopwatch();
        t.Start();
        foreach (var input in inputs)
        {
            yield return transform(input);
        }
        t.Stop();
        Console.WriteLine($"{TestClassName}.{callerName} Input transform:\t{t.ElapsedMilliseconds}ms");
    }
    
    protected IEnumerable<T> get_sample<T>(Func<string, T> transform, int partNumber = 1, string type = "day", [CallerMemberName] string callerName = "")
    {
        var inputs = get_sample(partNumber);
        var t = new Stopwatch();
        t.Start();
        foreach (var input in inputs)
        {
            yield return transform(input);
        }
        t.Stop();
        Console.WriteLine($"{TestClassName}.{callerName} Input transform:\t{t.ElapsedMilliseconds}ms");
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