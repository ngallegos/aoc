using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOC2015.Modules;

public class Day6 : DayBase
{
    public override bool Completed { get; }

    private static Regex _instructionRegex =
        new Regex(@"^(?<type>turn|toggle) ?(?<state>on|off)? (?<start>\d*,\d*) through (?<end>\d*,\d*)$"); 
    public override dynamic Part1()
    {
        var instructions = new List<LightingInstruction>();
        var instr = new LightingInstruction("turn on 0,0 through 999,999");
        var instr2 = new LightingInstruction("toggle 0,0 through 999,0");
        var instr3 = new LightingInstruction("turn off 499,499 through 500,500");

        return "not implemented";
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class LightingInstruction
    {
        public LightingInstruction(string input)
        {
            var result = _instructionRegex.Match(input);
            var type = result.Groups["type"].Value;
            var state = result.Groups["state"].Value;
            var start = result.Groups["start"].Value;
            var end = result.Groups["end"].Value;
        }
    }
}