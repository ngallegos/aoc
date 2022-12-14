using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day04 : DayBase
{
    public override bool Completed => true;
    
    public override dynamic Part1()
    {
        var assignmentPairs = get_input()
            .Select(x => new CleaningPair(x))
            .ToList();
        var pairsDuplicatingWork = assignmentPairs
            .Count(x => x.HasDuplicateAssignment);

        return new { pairsDuplicatingWork };
    }

    public override dynamic Part2()
    {
        var assignmentPairs = get_input()
            .Select(x => new CleaningPair(x))
            .ToList();
        var pairsWithOverlappingWork = assignmentPairs
            .Count(x => x.HasOverlappingAssignment);

        return new { pairsWithOverlappingWork };
    }

    private class CleaningAssignment
    {
        public CleaningAssignment(string input)
        {
            var sections = input.Split("-", StringSplitOptions.TrimEntries);
            StartingSection = int.Parse(sections[0]);
            EndingSection = int.Parse(sections[1]);
        }
        
        public int StartingSection { get; }
        public int EndingSection { get; }

        public bool ContainsAssignment(CleaningAssignment other) =>
            StartingSection <= other.StartingSection && EndingSection >= other.EndingSection;

        public bool OverlapsAssignment(CleaningAssignment other) => !HasNoOverlap(other);

        public bool HasNoOverlap(CleaningAssignment other) =>
            EndingSection < other.StartingSection || StartingSection > other.EndingSection;
    }

    private class CleaningPair
    {
        public CleaningPair(string input)
        {
            var assignments = input.Split(",", StringSplitOptions.TrimEntries);
            Elf1 = new CleaningAssignment(assignments[0]);
            Elf2 = new CleaningAssignment(assignments[1]);
        }
        public CleaningAssignment Elf1 { get; }
        public CleaningAssignment Elf2 { get; }

        public bool HasDuplicateAssignment => Elf1.ContainsAssignment(Elf2) || Elf2.ContainsAssignment(Elf1);
        public bool HasOverlappingAssignment => Elf1.OverlapsAssignment(Elf2) || Elf2.OverlapsAssignment(Elf1);
    }
}