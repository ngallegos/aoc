using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2015.Modules;

public class Day07 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var sampleCircuit = new Circuit(get_sample());
        var actualCircuit = new Circuit(get_input());

        return new { 
            sample = sampleCircuit.GetAllSignals()
                .OrderBy(x => x.wire)
                .Select(x => $"{x.wire}: {x.signal}"),
            
            actualAWireSignal = actualCircuit.GetSignal("a")
        };
    }

    public override dynamic Part2()
    {
        var actualCircuit = new Circuit(get_input());
        actualCircuit.Override("b", 3176);

        return new {
            actualAWireSignal = actualCircuit.GetSignal("a")
        };
    }

    private class Circuit
    {
        private List<Wire> _wires;

        public Circuit(IEnumerable<string> instructions)
        {
            _wires = instructions.Select(x => new Wire(x)).ToList();
        }

        public void Override(string wire, ushort signal)
        {
            _wires.First(x => x.ID == wire).SetSignal(signal);
        }

        public ushort GetSignal(string signal)
        {
            if (ushort.TryParse(signal, out var raw))
                return raw;
            return _wires.First(x => x.ID == signal).GetSignal(this);
        }

        public List<(string wire, ushort signal)> GetAllSignals()
        {
            return _wires.Select(w => (w.ID, w.GetSignal(this))).ToList();
        }
    }
    
    private class Wire
    {
        public string ID { get; private set; }
        private string _signalExpression;
        private ushort? _computedSignal;

        public Wire(string instruction)
        {
            var parts = instruction.Split(" -> ");
            ID = parts[1];
            _signalExpression = parts[0];
        }

        public void SetSignal(ushort signal)
        {
            _computedSignal = signal;
        }

        public ushort GetSignal(Circuit circuit)
        {
            if (_computedSignal.HasValue)
                return _computedSignal.Value;

            if (_signalExpression.Contains("AND"))
            {
                var signals = _signalExpression.Split(" AND ");
                _computedSignal = (ushort)(circuit.GetSignal(signals[0]) & circuit.GetSignal(signals[1]));
            }
            else if (_signalExpression.Contains("OR"))
            {
                var signals = _signalExpression.Split(" OR ");
                _computedSignal = (ushort)(circuit.GetSignal(signals[0]) | circuit.GetSignal(signals[1]));
            }
            else if (_signalExpression.Contains("LSHIFT"))
            {
                var signals = _signalExpression.Split(" LSHIFT ");
                _computedSignal = (ushort)(circuit.GetSignal(signals[0]) << int.Parse(signals[1]));
            }
            else if (_signalExpression.Contains("RSHIFT"))
            {
                var signals = _signalExpression.Split(" RSHIFT ");
                _computedSignal = (ushort)(circuit.GetSignal(signals[0]) >> int.Parse(signals[1]));
            }
            else if (_signalExpression.Contains("NOT"))
            {
                var signal = _signalExpression.Replace("NOT ", "");
                _computedSignal = (ushort)~circuit.GetSignal(signal);
            }
            else // its a raw signal
                _computedSignal = circuit.GetSignal(_signalExpression);

            return _computedSignal.Value;
        }
    }
}