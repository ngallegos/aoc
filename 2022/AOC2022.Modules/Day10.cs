using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day10 : DayBase
{
    public override bool Completed => true;

    public override dynamic Part1()
    {
        var sampleState = new DeviceState();
        sampleState.ProcessInput(get_sample().ToList());
        var actualState = new DeviceState();
        actualState.ProcessInput(get_input().ToList());
        var interestingCycles = new[] { 20, 60, 100, 140, 180, 220 };
        var sampleSum = sampleState.SumInterestingSignalStrengths(interestingCycles);
        var actualSum = actualState.SumInterestingSignalStrengths(interestingCycles);

        return new
        {
            Sample = new { sumOfInterestingSignalStrengths = sampleSum },
            ActuaL = new { sumOfInterestingSignalStrengths = actualSum }
        };
    }

    public override dynamic Part2()
    {
        var sampleState = new DeviceState();
        sampleState.ProcessInput(get_sample().ToList());
        var actualState = new DeviceState();
        actualState.ProcessInput(get_input().ToList());
        return new
        {
            Sample = sampleState.RenderImage(),
            Actual = actualState.RenderImage()
        };
    }

    private class DeviceState
    {
        private int _x { get; set; } = 1;
        private int _cycle { get; set; }
        private int _signalStrength => _cycle * _x;
        private List<int> _signalStrengths = new List<int>();
        private List<int> _xValues = new List<int>();

        public void ProcessInput(List<string> instructions)
        {
            foreach (var input in instructions)
            {
                var instruction = new CPUInstruction(input);
                ExecuteInstruction(instruction);
            }
        }
        
        private void ExecuteInstruction(CPUInstruction cpuInstruction)
        {
            for (int i = 0; i < cpuInstruction.CyclesToComplete; i++)
            {
                _cycle++;
                _signalStrengths.Add(_signalStrength);
                _xValues.Add(_x);
                if (i == cpuInstruction.CyclesToComplete -1)
                    _x = cpuInstruction.PerformOperation(_x);
            }
        }
        
        public int SumInterestingSignalStrengths(IEnumerable<int> cycleNumbers)
        {
            var sum = 0;
            foreach (var cycleNumber in cycleNumbers)
            {
                sum += _signalStrengths[cycleNumber - 1];
            }

            return sum;
        }

        public List<string> RenderImage()
        {
            var pixelLines = new List<string>();
            var currentLine = string.Empty;
            for (int i = 0; i < _xValues.Count; i++)
            {
                var spriteCenter = _xValues[i];
                var sprite = new List<int> { spriteCenter };
                if (spriteCenter > 0)
                    sprite.Insert(0, spriteCenter-1);
                if (spriteCenter < (_xValues.Count-1))
                    sprite.Add(spriteCenter+1);
                var linePosition = i % 40;
                var pixel = '.';
                if (sprite.Any(s => s == linePosition))
                    pixel = '#';
                if (linePosition == 0 && i != 0)
                {
                    pixelLines.Add(currentLine);
                    currentLine = string.Empty;
                }
                currentLine += pixel;
            }
            pixelLines.Add(currentLine);

            return pixelLines;
        }
    }
    
    private class CPUInstruction
    {
        private readonly int _arg;
        private readonly string _type;
        public int CyclesToComplete = 1;
        
        public CPUInstruction(string input)
        {
            var parts = input.Split(' ');
            _type = parts[0];
            if (_type == "addx")
            {
                CyclesToComplete = 2;
                _arg = Int32.Parse(parts[1]);
            }
        }

        public int PerformOperation(int currentValue)
        {
            CyclesToComplete = 0;
            return currentValue + _arg;
        }
    }
}