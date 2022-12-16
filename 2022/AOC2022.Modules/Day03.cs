using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day03 : DayBase
{
    public override bool Ignore => true;
    private List<(char letter, int priority)> PriorityMap = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        .ToCharArray()
        .Select((c, i) => (c, i+1)).ToList();

    public override dynamic Part1()
    {
        var priorityTotal = 0;
        var ruckSacks = get_input()
            .Select(x =>
            {
                var c1 = x.Substring(0, x.Length / 2);
                var c2 = x.Substring(x.Length / 2);
                return (c1, c2);
            }).ToList();
        foreach (var ruckSack in ruckSacks)
        {
            var commonItemtype = ruckSack.c1.Intersect(ruckSack.c2).First();
            var itemPriority = PriorityMap.First(x => x.letter == commonItemtype).priority;
            priorityTotal += itemPriority;
        }

        return new { priorityTotal };
    }

    public override dynamic Part2()
    {
        var priorityTotal = 0;
        var ruckSacks = get_input().ToList();
        var elfRuckSackGroups = ruckSacks.Chunk(3).ToList();
        foreach (var group in elfRuckSackGroups)
        {
            var commonItemtype = group[0].Intersect(group[1]).Intersect(group[2]).First();
            var itemPriority = PriorityMap.First(x => x.letter == commonItemtype).priority;
            priorityTotal += itemPriority;
        }

        return new { priorityTotal };
    }
}