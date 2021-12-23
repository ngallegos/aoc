using System;
using System.IO;
using System.Text.Json.Serialization;
using AOC2021.Modules;
using Newtonsoft.Json;

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
            
            System.Console.WriteLine($"\t{JsonConvert.SerializeObject(day0101)}");
            System.Console.WriteLine($"\t{JsonConvert.SerializeObject(day0102)}");

            System.Console.WriteLine("DAY 02");
            var day02 = new Day02();
            var day0201 = day02.Part1();
            var day0202 = day02.Part2();
            System.Console.WriteLine($"\tX: {day0201.xPosition}\tY: {day0201.yPosition}\tRESULT: {day0201.xPosition * day0201.yPosition}");
            System.Console.WriteLine($"\tX: {day0202.horizontalPosition}\tY: {day0202.depth}\tAIM:{day0202.aim}\tRESULT: {day0202.horizontalPosition * day0202.depth}");
            
        }
    }
}