using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    public interface IDaySolutions
    {
        int Day { get; }
        string Solution01();
        string Solution02();
    }

    public interface IDaySolutions<T> : IDaySolutions where T : IDayInputs
    {
        T Inputs { get; }
    }
}
