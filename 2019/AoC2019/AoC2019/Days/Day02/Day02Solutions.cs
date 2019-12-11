using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Days.Day02
{
    public class Day02Solutions : DaySolutionsBase<Day02Inputs>
    {
        public override int Day => 2;
        protected override string SolvePuzzle01(List<string> input)
        {
            var program = input.FirstOrDefault().Split(',').Select(Int32.Parse).ToList();
            return RunProgram(program, 12, 2).ToString();
        }

        protected override string SolvePuzzle02(List<string> input)
        {
            var program = input.FirstOrDefault().Split(',').Select(Int32.Parse).ToList();
            var result = 0;
            var target = 19690720;

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    result = RunProgram(program, i, j);
                    if (result == target)
                        return (100 * i + j).ToString();
                }
            }

            return "Something went wrong";
        }

        protected int RunProgram(List<int> inputPrgram, int noun, int verb)
        {
            var program = new int[inputPrgram.Count];
            inputPrgram.CopyTo(program);
            program[1] = noun;
            program[2] = verb;
            var index = 0;
            var opcode = program[index];

            while (opcode != 99 && index < program.Length)
            {
                switch (opcode)
                {
                    case 1:
                        program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]];
                        break;
                    case 2:
                        program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]];
                        break;
                    default:
                        throw new Exception($"Invalid opcode: {opcode} at index {index}");
                }

                index += 4;
                opcode = program[index];
            }

            return program[0];
        }
    }
}
