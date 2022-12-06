﻿using System.Linq;

namespace AOC2022.Modules;

public class Day06 : DayBase
{
    private readonly string _input;
    public Day06() => _input = get_input("Part1").First();
    public override dynamic Part1()
    {
        return new { charactersProcessed = DetermineEndOfFirstMarker(_input, 4) };
    }

    public override dynamic Part2()
    {
        var input = get_input().First();
        return new { charactersProcessed = DetermineEndOfFirstMarker(_input, 14) };
    }

    private int DetermineEndOfFirstMarker(string input, int markerSize)
    {
        for (int i = 0; i < (input.Length - markerSize); i++)
        {
            var buffer = input.Substring(i, markerSize);
            if (buffer.Distinct().Count() == markerSize)
                return i + markerSize;
        }

        return input.Length;
    }
}