using Shouldly;

namespace AOC2023.Tests;

/// <summary>
/// https://www.reddit.com/r/adventofcode/comments/18e5ytd/comment/kco0074/
/// </summary>
public class Day09Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var answer = get_sample(x => 
            x.Split(' ', StringSplitOptions.TrimEntries)
                .Select(int.Parse))
            .Select(x => generateNextValue(x).Last())
            .Sum();
        
        answer.ShouldBe(114);
    }

    protected override void SolvePart2_Sample()
    {
        var answer = get_sample(x => 
                x.Split(' ', StringSplitOptions.TrimEntries)
                    .Select(int.Parse).Reverse())
            .Select(x => generateNextValue(x).Last())
            .Sum();
        
        answer.ShouldBe(2);
    }

    protected override void SolvePart1_Actual()
    {
        var answer = get_input(x => 
                x.Split(' ', StringSplitOptions.TrimEntries)
                    .Select(int.Parse))
            .Select(x => generateNextValue(x).Last())
            .Sum();
        
        answer.ShouldBe(1853145119);
    }

    protected override void SolvePart2_Actual()
    {
        var answer = get_input(x => 
                x.Split(' ', StringSplitOptions.TrimEntries)
                    .Select(int.Parse).Reverse())
            .Select(x => generateNextValue(x).Last())
            .Sum();
        
        answer.ShouldBe(2);
    }
    
    
    IEnumerable<int> generateNextValue(IEnumerable<int> input) {
        if (input.All(i => i == 0)) { return input; }
        var nextLine = generateNextValue(Pairwise(input).Select(pair => pair.Item2 - pair.Item1));
        return input.Append(nextLine.Last() + input.Last());
    }

    public static IEnumerable<(T, T)> Pairwise<T>(IEnumerable<T> source)
    {
        var previous = default(T);
        using var it = source.GetEnumerator();
        if (it.MoveNext()) previous = it.Current;
        while (it.MoveNext()) yield return (previous, previous = it.Current);
    }
}