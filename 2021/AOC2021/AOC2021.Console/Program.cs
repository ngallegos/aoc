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
            System.Console.WriteLine($"Increases: {results.increased}\nDecreases: {results.decreased}");
        }
    }
}