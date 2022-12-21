using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day21 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var sampleBarrel = new Barrel(get_sample());
        var actualBarrel = new Barrel(get_input());

        return new
        {
            sample = sampleBarrel.GetYell("root"),
            actual = actualBarrel.GetYell("root")
        };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }
    
    private class Barrel
    {
        private List<Monkey> _monkeys;

        public Barrel(IEnumerable<string> instructions)
        {
            _monkeys = instructions.Select(x => new Monkey(x)).ToList();
        }

        public void Override(string wire, long signal)
        {
            _monkeys.First(x => x.ID == wire).SetSignal(signal);
        }

        public long GetYell(string expression)
        {
            if (long.TryParse(expression, out var raw))
                return raw;
            return _monkeys.First(x => x.ID == expression).GetYell(this);
        }

        public List<(string monkey, long yell)> GetAllYells()
        {
            return _monkeys.Select(w => (w.ID, w.GetYell(this)))
                .OrderBy(x => x.ID)
                .ToList();
        }
    }
    
    private class Monkey
    {
        public string ID { get; private set; }
        private string _job;
        private long? _yell;

        public Monkey(string instruction)
        {
            var parts = instruction.Split(": ");
            ID = parts[0];
            _job = parts[1];
        }

        public void SetSignal(long signal)
        {
            _yell = signal;
        }

        public long GetYell(Barrel barrel)
        {
            if (_yell.HasValue)
                return _yell.Value;

            if (_job.Contains("+"))
            {
                var signals = _job.Split(" + ");
                _yell = barrel.GetYell(signals[0]) + barrel.GetYell(signals[1]);
            }
            else if (_job.Contains("-"))
            {
                var signals = _job.Split(" - ");
                _yell = barrel.GetYell(signals[0]) - barrel.GetYell(signals[1]);
            }
            else if (_job.Contains("*"))
            {
                var signals = _job.Split(" * ");
                _yell = barrel.GetYell(signals[0]) * barrel.GetYell(signals[1]);
            }
            else if (_job.Contains("/"))
            {
                var signals = _job.Split(" / ");
                _yell = barrel.GetYell(signals[0]) / barrel.GetYell(signals[1]);
            }
            else // its a raw signal
                _yell = barrel.GetYell(_job);

            return _yell.Value;
        }
    }
}