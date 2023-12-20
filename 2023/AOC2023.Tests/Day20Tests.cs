namespace AOC2023.Tests;

public class Day20Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Sample()
    {
        throw new NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new NotImplementedException();
    }

    private abstract class Module
    {
        protected List<Module> Inputs { get; private set; } = new();
        protected List<Module> Outputs { get; private set; } = new();
        public event EventHandler<InputProcessedEventArgs> InputProcessed;

        protected abstract Pulse Process(Pulse inputPulse);
        public void ProcessInput(Pulse inputPulse)
        {
            var outputPulse = Process(inputPulse);
            InputProcessed?.Invoke(this, new InputProcessedEventArgs
            {
                Module = this,
                InputPulse = inputPulse,
                OutputPulse = outputPulse
            });
        }
    }

    private class InputProcessedEventArgs : EventArgs
    {
        public Module Module { get; set; }
        public Pulse InputPulse { get; set; }
        public Pulse OutputPulse { get; set; }
    }
    private enum Pulse
    {
        High,
        Low,
    }
}