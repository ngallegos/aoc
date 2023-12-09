using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day01Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var expectedCalibrationSum = 142;
        var calibrationValues = get_sample()
            .Select(GetNumberFromFirstAndLastDigitsInString).ToList();

        // Act
        var calibrationSum = calibrationValues.Sum();
        
        // Assert
        calibrationSum.ShouldBe(expectedCalibrationSum);
    }
    
    protected override void SolvePart1_Actual()
    {
        // Arrange
        var expectedCalibrationSum = 54601;
        var calibrationValues = get_input()
            .Select(GetNumberFromFirstAndLastDigitsInString).ToList();

        // Act
        var calibrationSum = calibrationValues.Sum();
        
        // Assert
        calibrationSum.ShouldBe(expectedCalibrationSum);
    }

    private int GetNumberFromFirstAndLastDigitsInString(string input)
    {
        var justNumbers = Regex.Replace(input, @"[^\d]", "");
        var charArray = new char[]{ justNumbers.First(), justNumbers.Last()};
        var firstAndLast = new string(charArray);
        return int.Parse(firstAndLast);
    }
    
    protected override void SolvePart2_Sample()
    {
        // Arrange
        var expectedCalibrationSum = 281;
        var calibrationValues = get_sample(2)
            .Select(GetNumberFromFirstAndLastDigitsOrSpellingsInString).ToList();

        // Act
        var calibrationSum = calibrationValues.Sum();
        
        // Assert
        calibrationSum.ShouldBe(expectedCalibrationSum);
    }
    
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var expectedCalibrationSum = 54078;
        var calibrationValues = get_input()
            .Select(GetNumberFromFirstAndLastDigitsOrSpellingsInString).ToList();

        // Act
        var calibrationSum = calibrationValues.Sum();
        
        // Assert
        calibrationSum.ShouldBe(expectedCalibrationSum);
    }
    
    private int GetNumberFromFirstAndLastDigitsOrSpellingsInString(string input)
    {
        var spellings = _spelledNumberMap.Select(x => x.spelling);
        var numRegex = new Regex($@"(\d|{string.Join("|", spellings)})");
        var matches = new List<Match>();
        matches.Add(numRegex.Match(input));
        while (matches.Last().Success) {
            matches.Add(numRegex.Match(input, matches.Last().Index + 1)); 
        }
        matches.RemoveAt(matches.Count - 1);
        var firstMatch = matches.First().Value;
        var lastMatch = matches.Last().Value;
        var firstNumber = _spelledNumberMap.FirstOrDefault(x => x.spelling == firstMatch).value;
        firstNumber = firstNumber == 0 ? int.Parse(firstMatch) : firstNumber;
        var lastNumber = _spelledNumberMap.FirstOrDefault(x => x.spelling == lastMatch).value;
        lastNumber = lastNumber == 0 ? int.Parse(lastMatch) : lastNumber;
        return firstNumber * 10 + lastNumber;
    }

    private List<(string spelling, int value)> _spelledNumberMap = new List<(string spelling, int value)>
    {
        ("one", 1),
        ("two", 2),
        ("three", 3),
        ("four", 4),
        ("five", 5),
        ("six", 6),
        ("seven", 7),
        ("eight", 8),
        ("nine", 9)
    };
}