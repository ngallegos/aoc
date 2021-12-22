using System;
using System.IO;
using AOC2021.Modules;

namespace AOC2021.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("DAY 01");
            var day01 = new Day01();
            var day0101 = day01.Part1();
            var day0102 = day01.Part2();
            System.Console.WriteLine($"\tIncreases: {day0101.increased}\tDecreases: {day0101.decreased}");
            System.Console.WriteLine($"\tIncreases: {day0102.increased}\tDecreases: {day0102.decreased}");

            System.Console.WriteLine("DAY 02");
            var day02 = new Day02();
            var day0201 = day02.Part1();
            var day0202 = day02.Part2();
            System.Console.WriteLine($"\tX: {day0201.xPosition}\tY: {day0201.yPosition}\tRESULT: {day0201.xPosition * day0201.yPosition}");
            System.Console.WriteLine($"\tX: {day0202.horizontalPosition}\tY: {day0202.depth}\tAIM:{day0202.aim}\tRESULT: {day0202.horizontalPosition * day0202.depth}");
            
        }
    }
}