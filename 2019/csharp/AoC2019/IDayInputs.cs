using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    public interface IDayInputs
    {
        int Day { get;  }
        List<string> Input01 { get; }
        List<string> Input02 { get; }
    }
}
