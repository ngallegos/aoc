using System.Text.RegularExpressions;

namespace AOC2023.Tests;

public class Day20Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var modules = get_sample(Module.Create)
            .ToList();
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

    private class FlipFlop : Module
    {
        protected override Pulse Process(Pulse inputPulse)
        {
            throw new NotImplementedException();
        }
    }
    
    private class Conjunction : Module
    {
        protected override Pulse Process(Pulse inputPulse)
        {
            throw new NotImplementedException();
        }
    }
    
    private class Broadcast : Module
    {

        protected override Pulse Process(Pulse inputPulse)
        {
            throw new NotImplementedException();
        }
    }
    
    private abstract class Module
    {
        public string ID { get; set; }
        protected List<Module> Inputs { get; set; } = new();
        protected List<Module> Outputs { get; set; } = new();
        public string[] OutputIDS = Array.Empty<string>();
        public event EventHandler<InputProcessedEventArgs> InputProcessed;

        private static Regex _definitionRegex = new Regex(@"^(?<type>[%&]?)(?<id>[a-z]+) -> (?<outputs>.*)$");

        public static Module Create(string definition)
        {
            var match = _definitionRegex.Match(definition);
            if (!match.Success)
                throw new ArgumentException($"Invalid definition: {definition}");
            var id = match.Groups["id"].Value;
            var outputs = match.Groups["outputs"].Value.Split(", ", StringSplitOptions.TrimEntries);
            Type type;
            switch (match.Groups["type"].Value)
            {
                case "%":
                    type = typeof(FlipFlop);
                    break;
                case "&":
                    type = typeof(Conjunction);
                    break;
                default:
                    type = typeof(Broadcast);
                    break;
            }

            var module = Activator.CreateInstance(type) as Module;
            if (module == null)
                throw new Exception("Could not create module");
            module.ID = id;
            module.OutputIDS = outputs;
            return module;
        }

        public void AssignInputs(List<Module> inputs)
        {
            
        }
        
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