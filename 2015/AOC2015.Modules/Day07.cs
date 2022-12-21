using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2015.Modules;

public class Day07 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class Circuit
    {
        private List<Wire> _wires = new List<Wire>();
        
        

        public void ParseInstruction(string instruction)
        {
            var parts = instruction.Split(" -> ");
            var wire = _wires.FirstOrDefault(x => x.ID == parts[1]);
            if (wire == null)
            {
                wire = new Wire(new Signal(parts[0])) { ID = parts[1] };
                _wires.Add(wire);
            }
        }
    }
    
    private class Wire
    {
        public string ID { get; set; }
        private Signal _signal { get; set; }

        public Wire(Signal signal)
        {
            _signal = signal;
        }
    }

    private class Signal
    {
        public bool Active => _getSignal?.Invoke() != null;

        private Wire _leftWire = null;
        private Wire _rightWire = null;

        private Func<int> _getSignal;

        public Signal(string input)
        {
            if (int.TryParse(input, out int rawsignal))
            {
                _getSignal = () => rawsignal;
            }
        }
    }
}