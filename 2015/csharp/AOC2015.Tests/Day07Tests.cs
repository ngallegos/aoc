namespace AOC2015.Tests;

public class Day07Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var circuit = new Circuit(get_sample());

        // Act
        var signals = circuit.GetAllSignals()
            .OrderBy(x => x.wire)
            .Select(x => $"{x.wire}: {x.signal}");

        // Assert
        signals.ShouldBe([
            "d: 72",
            "e: 507",
            "f: 492",
            "g: 114",
            "h: 65412",
            "i: 65079",
            "x: 123",
            "y: 456"
        ]);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var circuit = new Circuit(get_input());

        // Act
        var signal = circuit.GetSignal("a");

        // Assert
        signal.ShouldBe((ushort)3176);
    }

    [Ignore("No sample")]
    protected override void SolvePart2_Sample()
    {
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var part1Signal = new Circuit(get_input()).GetSignal("a");
        var circuit = new Circuit(get_input());

        // Act
        circuit.Override("b", part1Signal);
        var signal = circuit.GetSignal("a");

        // Assert
        signal.ShouldBe((ushort)14710);
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
