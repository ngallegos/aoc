using System;
using System.IO;
using AOC2021.Modules;

namespace AOC2021.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day01();
            var results = day.Part1();
            var results2 = day.Part2();
            System.Console.WriteLine($"Increases: {results.increased}\nDecreases: {results.decreased}");
            System.Console.WriteLine($"Increases: {results2.increased}\nDecreases: {results2.decreased}");
        }
    }
}