using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// https://www.reddit.com/r/adventofcode/comments/zrav4h/2022_day_21_solutions/
    /// </summary>
    /// <returns></returns>
    public override dynamic Part2()
    {
        var sampleBarrel = new Barrel(get_sample());
        sampleBarrel.OverrideJobOperator("root", "=");
        var sampleHumanYell = 0;
        sampleBarrel.OverrideJob("humn", sampleHumanYell.ToString());
        var sampleRootYell = sampleBarrel.GetYell("root");
        while (sampleRootYell != 0)
        {
            sampleHumanYell++;
            sampleBarrel.Reset();
            sampleBarrel.OverrideJob("humn", sampleHumanYell.ToString());
            sampleRootYell = sampleBarrel.GetYell("root");
        }
        
        var actualBarrel = new Barrel(get_input());
        actualBarrel.OverrideJobOperator("root", "=");
        var actualHumanYell = 0;
        actualBarrel.OverrideJob("humn", actualHumanYell.ToString());
        var actualRootYell = actualBarrel.GetYell("root");
        var increment = 1000000;
        while (actualRootYell != 0)
        {
            actualHumanYell += increment;
            actualBarrel.Reset();
            actualBarrel.OverrideJob("humn", actualHumanYell.ToString());
            var curYell = actualBarrel.GetYell("root");
            if (actualRootYell < curYell)
            {
                increment = -increment / 10;
                
                Console.WriteLine($"increment: {increment}");
            }
            actualRootYell = curYell;
        }
       
        return new
        {
            sample = sampleHumanYell,
            actual = actualHumanYell
        };
    }
    
    private class Barrel
    {
        private List<Monkey> _monkeys;

        public Barrel(IEnumerable<string> instructions)
        {
            _monkeys = instructions.Select(x => new Monkey(x)).ToList();
        }

        public void OverrideJobOperator(string monkey, string newOperator)
        {
            var m = _monkeys.Find(x => x.ID == monkey);
            m.SetJob(Regex.Replace(m.Job, @"[+\-*\/]", newOperator));
        }

        public void OverrideJob(string monkey, string job)
        {
            _monkeys.Find(x => x.ID == monkey).SetJob(job);
        }

        public long GetYell(string expression)
        {
            if (long.TryParse(expression, out var raw))
                return raw;
            return _monkeys.First(x => x.ID == expression).GetYell(this);
        }

        public void Reset()
        {
            _monkeys.ForEach(m => m.ClearYell());
        }
    }
    
    private class Monkey
    {
        public string ID { get; private set; }
        public string Job => _job;
        private string _job;
        private long? _yell;

        public Monkey(string instruction)
        {
            var parts = instruction.Split(": ");
            ID = parts[0];
            _job = parts[1];
        }

        public void SetJob(string job)
        {
            _job = job;
        }

        public void ClearYell()
        {
            _yell = null;
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
            else if (_job.Contains("="))
            {
                var signals = _job.Split(" = ");
                _yell = barrel.GetYell(signals[0]) - barrel.GetYell(signals[1]); // if 0, means they're equal
            }
            else // its a raw signal
                _yell = barrel.GetYell(_job);

            return _yell.Value;
        }
    }
}