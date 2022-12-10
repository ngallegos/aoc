using System;
using System.Collections.Generic;

namespace AOC2022.Modules;

public class Day10 : DayBase
{
    public override bool Completed => false;

    public override dynamic Part1()
    {
        var sample = get_sample();
        var inputs = get_input();
        var state = new ExecutionState();
        foreach (var input in inputs)
        {
            var instruction = new CPUInstruction(input);
            state.ExecuteInstruction(instruction);
        }

        var interestingCycles = new[] { 20, 60, 100, 140, 180, 220 };
        var sumOfInterestingSignalStrengths = state.SumInterestingSignalStrengths(interestingCycles);

        return new { sumOfInterestingSignalStrengths };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class ExecutionState
    {
        private int _x { get; set; } = 1;
        private int _cycle { get; set; }
        private int _signalStrength => _cycle * _x;
        private List<int> _signalStrengths = new List<int>();
        private List<CPUInstruction> _executingInstructions = new List<CPUInstruction>();

        public void ExecuteInstruction(CPUInstruction cpuInstruction)
        {
            for (int i = 0; i < cpuInstruction.CyclesToComplete; i++)
            {
                _cycle++;
                _signalStrengths.Add(_signalStrength);
            }
            _x = cpuInstruction.PerformOperation(_x);
        }
        
        public void ExecuteInstructionAsync(CPUInstruction cpuInstruction)
        {
            _cycle++;
            _signalStrengths.Add(_signalStrength);
            _executingInstructions.Add(cpuInstruction);
            foreach (var instruction in _executingInstructions)
            {
                _x = instruction.PerformOperationAsync(_x);
            }
            _executingInstructions.RemoveAll(x => x.Complete);
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
    }
    
    private class CPUInstruction
    {
        private readonly int _arg;
        private readonly string _type;
        public int CyclesToComplete = 1;
        public bool Complete => CyclesToComplete <= 0;
        
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

        public int PerformOperationAsync(int currentValue)
        {
            CyclesToComplete--;
            if (CyclesToComplete == 0)
                return currentValue + _arg;
            return currentValue;
        }
    }
}