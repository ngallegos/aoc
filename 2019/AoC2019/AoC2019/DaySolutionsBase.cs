using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    public abstract class DaySolutionsBase<T> : IDaySolutions<T> where T : IDayInputs
    {
        public T Inputs => Activator.CreateInstance<T>();
        public abstract int Day { get; }

        public string Solution01()
        {
            try
            {
                return SolvePuzzle01(Inputs.Input01);
            }
            catch
            {
                return $"Day {Inputs.Day:D2} Solution 01 Not Implemented";
            }
        }

        public string Solution02()
        {
            try
            {
                return SolvePuzzle02(Inputs.Input02);
            }
            catch
            {
                return $"Day {Inputs.Day:D2} Solution 01 Not Implemented";
            }
        }

        protected abstract string SolvePuzzle01(List<string> input);
        protected abstract string SolvePuzzle02(List<string> input);

    }
}
