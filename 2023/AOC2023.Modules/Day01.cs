using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2023.Modules
{
    public class Day01 : DayBase
    {
        public override bool Ignore => false;

        public override dynamic Part1()
        {
            var calibrationValues = get_input()
                .Select(GetNumberFromFirstAndLastDigitsInString).ToList();
            return new { calibrationSum = calibrationValues.Sum() };
        }

        public override dynamic Part2()
        {
            throw new NotImplementedException();
        }

        private int GetNumberFromFirstAndLastDigitsInString(string input)
        {
            var justNumbers = Regex.Replace(input, @"[^\d]", "");
            var charArray = new char[]{ justNumbers.First(), justNumbers.Last()};
            var firstAndLast = new string(charArray);
            return int.Parse(firstAndLast);
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
            ("eight", 9),
            ("nine", 9)
        };
    }
}