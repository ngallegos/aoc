using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2025.Tests;

public class Day01Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var input = get_sample();
        var dial = new Dial();
        
        // Act
        dial.Rotate(input);
        
        // Assert
        input.ShouldNotBeEmpty();
        dial.TimesAtZero.ShouldBe(3);
    }
    
    protected override void SolvePart1_Actual()
    {
        // Arrange
        var input = get_input();
        var dial = new Dial();
        
        // Act
        dial.Rotate(input);
        
        // Assert
        input.ShouldNotBeEmpty();
        dial.TimesAtZero.ShouldBe(1048);
    }
    
    protected override void SolvePart2_Sample()
    {
        // Arrange
        var input = get_sample();
        var dial = new Dial(countTimesPassingZero: true);
        
        // Act
        dial.Rotate(input);
        
        // Assert
        input.ShouldNotBeEmpty();
        dial.TimesAtZero.ShouldBe(6);
    }
    
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var input = get_input();
        var dial = new Dial(countTimesPassingZero: true);
        
        // Act
        dial.Rotate(input);
        
        // Assert
        input.ShouldNotBeEmpty();
        dial.TimesAtZero.ShouldBe(6498);
    }

    private class Dial(bool countTimesPassingZero = false, int currentPosition = 50, int maxPosition = 99)
    {
        private int _currentPosition = currentPosition;
        private int _timesAtZero = 0;
        
        public int TimesAtZero => _timesAtZero;
        public int CurrentPosition => _currentPosition;

        void Rotate(string instruction)
        {
            var match = Regex.Match(instruction, @"(L|R)(\d+)");
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid instruction: {instruction}");
            }

            var direction = match.Groups[1].Value;
            var clicksRemaining = int.Parse(match.Groups[2].Value);
            var clicksAtZero = 0;

            while (clicksRemaining > 0)
            {
                if (direction == "R")
                {
                    _currentPosition--;
                    if (_currentPosition < 0)
                    {
                        _currentPosition = maxPosition;
                    }
                }
                else if (direction == "L")
                {
                    _currentPosition++;
                    if (_currentPosition > maxPosition)
                    {
                        _currentPosition = 0;
                    }
                }
                
                if (_currentPosition == 0)
                {
                    clicksAtZero++;
                }
                
                clicksRemaining--;
            }
            


            if (countTimesPassingZero)
            {
                _timesAtZero += clicksAtZero;
            } 
            else if (_currentPosition == 0)
            {
                _timesAtZero++;
            }
        }
        
        public void Rotate(IEnumerable<string> instructions)
        {
            foreach (var instruction in instructions)
            {
                Rotate(instruction);
            }
        }
    }
}