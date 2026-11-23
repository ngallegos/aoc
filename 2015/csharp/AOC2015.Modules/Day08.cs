using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC2015.Modules;

public class Day08 : DayBase
{
    public override bool Ignore => true;
    private static Regex _hexRegex = new Regex(@"\\x[a-fA-F0-9]{2}");
    public override dynamic Part1()
    {
        var sample = get_sample().ToList();
        var sampleParsed = sample.Select(ParsePart1)
            .ToList();
        var sampleDiff = sample.Sum(x => x.Length) - sampleParsed.Sum(x => x.Length);

        var actual = get_input().ToList();
        var actualParsed = actual.Select(ParsePart1).ToList();
        var actualDiff = actual.Sum(x => x.Length) - actualParsed.Sum(x => x.Length);
        

        return new
        {
            sampleDiff,
            actualDiff
        };
    }

    public override dynamic Part2()
    {
        var sample = get_sample().ToList();
        var sampleParsed = sample.Select(ParsePart2)
            .ToList();
        var sampleDiff = sampleParsed.Sum(x => x.Length) - sample.Sum(x => x.Length) ;

        var actual = get_input().ToList();
        var actualParsed = actual.Select(ParsePart2).ToList();
        var actualDiff = actualParsed.Sum(x => x.Length) - actual.Sum(x => x.Length);
        

        return new
        {
            sample,
            sampleParsed,
            sampleDiff,
            actualDiff
        };
    }

    private string ParsePart1(string input)
    {
        var result = input.Trim('"');
        result = result.Replace("\\\"","\"");
        result = result.Replace("\\\\", "\\");
        var hexMatches = _hexRegex.Matches(result);
        foreach (Match hexMatch in hexMatches)
        {
            result = result.Replace(hexMatch.Value, HexToAscii(hexMatch.Value));
        }
        return result;
    }
    
    private string ParsePart2(string input)
    {
        var result = input; 
        result = result.Replace("\\", "\\\\");
        result = result.Replace("\"", "\\\"");
        return "\"" + result + "\"";
    }

    private string HexToAscii(string hex)
    {
        hex = hex.Replace("\\x", "");
        var hexByte = Convert.ToByte(hex, 16);
        return Encoding.ASCII.GetString(new[] { hexByte });
    }
}