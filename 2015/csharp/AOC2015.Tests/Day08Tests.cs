using System.Text;
using System.Text.RegularExpressions;

namespace AOC2015.Tests;

public class Day08Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var strings = get_sample().ToList();

        // Act
        var diff = strings.Sum(x => x.Length) - strings.Sum(x => Decode(x).Length);

        // Assert
        diff.ShouldBe(12);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var strings = get_input().ToList();

        // Act
        var diff = strings.Sum(x => x.Length) - strings.Sum(x => Decode(x).Length);

        // Assert
        diff.ShouldBe(1333);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var strings = get_sample().ToList();

        // Act
        var diff = strings.Sum(x => Encode(x).Length) - strings.Sum(x => x.Length);

        // Assert
        diff.ShouldBe(19);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var strings = get_input().ToList();

        // Act
        var diff = strings.Sum(x => Encode(x).Length) - strings.Sum(x => x.Length);

        // Assert
        diff.ShouldBe(2046);
    }

    private static Regex _hexRegex = new Regex(@"\\x[a-fA-F0-9]{2}");
    private string Decode(string input)
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
    
    private string Encode(string input)
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
