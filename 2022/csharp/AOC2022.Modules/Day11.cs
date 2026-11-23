using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day11 : DayBase
{
    public override bool Ignore => true;

    public override dynamic Part1()
    {
        var rounds = 20;
        var reliefManagement = (long x) => (long)Math.Floor((double)x / 3);
        var _sampleBarrel = new Barrel(get_sample().ToList());
        var _actualBarrel = new Barrel(get_input().ToList());
        _sampleBarrel.ConductMonkeyBusiness(rounds, reliefManagement);
        _actualBarrel.ConductMonkeyBusiness(rounds, reliefManagement);
       

        return new
        {
            sampleMonkeyBusiness = _sampleBarrel.BusinessLevel,
            actualMonkeyBusiness = _actualBarrel.BusinessLevel
        };
    }

    public override dynamic Part2()
    {
        var rounds = 10000;
        var _sampleBarrel = new Barrel(get_sample().ToList());
        var _actualBarrel = new Barrel(get_input().ToList());
        _sampleBarrel.ConductMonkeyBusiness(rounds, (long x) => x % _sampleBarrel.StressReducer);
        _actualBarrel.ConductMonkeyBusiness(rounds, (long x) => x % _actualBarrel.StressReducer);
       

        return new
        {
            sampleMonkeyBusiness = _sampleBarrel.BusinessLevel,
            actualMonkeyBusiness = _actualBarrel.BusinessLevel
        };
    }

    private class Barrel
    {
        private List<Monkey> _monkeys;
        private readonly List<string> _notes;

        public long BusinessLevel => _monkeys.OrderByDescending(x => x.InspectionsPerformed)
            .Select(x => x.InspectionsPerformed)
            .Take(2)
            .Aggregate((a, b) => a * b);

        public long StressReducer => _monkeys
            .Select(x => x.TestDivisibleBy)
            .Aggregate((a, b) => a * b);
        
        
        public Barrel(List<string> notes)
        {
            _notes = notes;
            _monkeys = new List<Monkey>();
            var currentMonkeyNotes = new List<string>();
            foreach (var note in _notes)
            {
                if (string.IsNullOrEmpty(note.Trim()))
                {
                    _monkeys.Add(new Monkey(currentMonkeyNotes.ToList()));
                    currentMonkeyNotes.Clear();
                }
                else
                    currentMonkeyNotes.Add(note);
            }
            _monkeys.Add(new Monkey(currentMonkeyNotes.ToList()));
        }

        public void ConductMonkeyBusiness(int rounds, Func<long, long> manageRelief)
        {
            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in _monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        monkey.PerformInspection();
                        monkey.ApplyRelief(manageRelief);
                        var destinationMonkey = monkey.DetermineNextMonkey();
                        monkey.Throw(_monkeys[destinationMonkey]);
                    }
                }
            }
        }
    }
    
    private class Monkey
    {
        public int ID { get; set; }
        public List<long> Items { get; private set; } = new List<long>();
        private Operation _operation;
        public long TestDivisibleBy { get; private set; }
        private int _testEvaluationTrueDestinationMonkey;
        private int _testEvaluationFalseDestinationMonkey;
        public long InspectionsPerformed { get; private set; }
        
        public Monkey(List<string> notes)
        {
            ID = int.Parse(notes[0].Replace("Monkey ", "").TrimEnd(':'));
            Items = notes[1].Trim().Replace("Starting items: ", "").Split(',', StringSplitOptions.TrimEntries)
                .Select(long.Parse).ToList();
            _operation = new Operation(notes[2].Trim().Replace("Operation: ", ""));
            TestDivisibleBy = int.Parse(notes[3].Trim().Replace("Test: divisible by ", ""));
            _testEvaluationTrueDestinationMonkey = int.Parse(notes[4].Trim().Replace("If true: throw to monkey ", ""));
            _testEvaluationFalseDestinationMonkey = int.Parse(notes[5].Trim().Replace("If false: throw to monkey ", ""));
        }

        public void PerformInspection()
        {
            Items[0] = _operation.Execute(Items[0]);
            InspectionsPerformed++;
        }

        public void ApplyRelief(Func<long, long> manageRelief)
        {
            Items[0] = manageRelief(Items[0]);
        }

        public void CatchItem(long itemWorryLevel)
        {
            Items.Add(itemWorryLevel);
        }

        public void Throw(Monkey destinationMonkey)
        {
            var item = Items.First();
            Items.RemoveAt(0);
            destinationMonkey.CatchItem(item);
        }

        public int DetermineNextMonkey()
        {
            if (Items[0] % TestDivisibleBy == 0)
                return _testEvaluationTrueDestinationMonkey;
            return _testEvaluationFalseDestinationMonkey;
        }
    }

    private class Operation
    {
        private string _leftArg;
        private string _rightArg;
        private string _operation;
        public Operation(string expression)
        {
            expression = expression.Replace("new = ", "");
            var parts = expression.Split(' ');
            _leftArg = parts[0];
            _operation = parts[1];
            _rightArg = parts[2];
        }

        public long Execute(long currentValue)
        {
            var left = _leftArg == "old" ? currentValue : long.Parse(_leftArg);
            var right = _rightArg == "old" ? currentValue : long.Parse(_rightArg);
            switch (_operation)
            {
                case "+":
                    return left + right;
                case "*":
                    return left * right;
            }

            return currentValue; // shouldn't happen
        }
    }
}