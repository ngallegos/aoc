using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day20Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var input = get_sample();
        var result = GetProductOfHighAndLowPulses(input);
        result.ShouldBe(11687500L);
    }

    protected override void SolvePart1_Actual()
    {
        var input = get_input();
        var result = GetProductOfHighAndLowPulses(input);
        result.ShouldBe(11687500L);
    }

    private long GetProductOfHighAndLowPulses(IEnumerable<string> input)
    {
        var modules = input.Select(Module.Create)
            .ToList();
        modules.Insert(0, new Button());
        modules.Add(new Output());
        modules.ForEach(m => m.LinkInputsAndOutputs(modules));
        for (int i = 0; i < 1000; i++)
        {
            var outgoingPulses = new Queue<(string inputID, Module output, Pulse pulse)>();
            var button = modules.First(x => x.ID == "button");

            outgoingPulses.Enqueue(button.ProcessInput(button.ID, Pulse.Low).First());
            while (outgoingPulses.Any())
            {
                var pulseToProcess = outgoingPulses.Dequeue();
                var outputs = pulseToProcess.output.ProcessInput(pulseToProcess.inputID, pulseToProcess.pulse);
                foreach (var output in outputs)
                    outgoingPulses.Enqueue(output);
            }
        }

        long totalLowPulses = modules.Select(x => x.LowPulsesSent).Sum();
        long totalHighPulses = modules.Select(x => x.HighPulsesSent).Sum();
        return totalHighPulses * totalLowPulses;
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
        private bool _isOn = false;
        protected override Pulse? Process(string inputID, Pulse inputPulse)
        {
            if (inputPulse == Pulse.High)
                return null;
            _isOn = !_isOn;
            if (_isOn)
                return Pulse.High;
            return Pulse.Low;
        }
    }
    
    private class Conjunction : Module
    {
        private Dictionary<string, Pulse> _lastInputPulses = new();
        
        public Conjunction()
        {
            InputsLinked += OnInputsLinked;
        }

        private void OnInputsLinked(object? sender, EventArgs e)
        {
            _lastInputPulses = Inputs.ToDictionary(x => x.ID, _ => Pulse.Low);
        }

        protected override Pulse? Process(string inputID, Pulse inputPulse)
        {
            _lastInputPulses[inputID] = inputPulse;
            if (_lastInputPulses.Values.All(x => x == Pulse.High))
                return Pulse.Low;
            return Pulse.High;
        }
    }
    
    private class Broadcast : Module
    {
        protected override Pulse? Process(string inputID, Pulse inputPulse)
        {
            return inputPulse;
        }
    }
    
    private class Output : Module
    {
        public Output()
        {
            ID = "output";
            OutputIDS = new[] { "broadcaster" };
        }
        protected override Pulse? Process(string inputID, Pulse inputPulse)
        {
            return null;
        }
    }

    private class Button : Module
    {
        public Button()
        {
            ID = "button";
            OutputIDS = new[] { "broadcaster" };
        }


        protected override Pulse? Process(string inputID, Pulse inputPulse)
        {
            return Pulse.Low;
        }
    }
    
    private abstract class Module
    {
        public string ID { get; set; }
        protected List<Module> Inputs { get; set; } = new();
        protected List<Module> Outputs { get; set; } = new();
        public string[] OutputIDS = Array.Empty<string>();
        public int HighPulsesSent { get; private set; }
        public int LowPulsesSent { get; private set; }
        public event EventHandler<InputProcessedEventArgs> InputProcessed;
        public event EventHandler InputsLinked;

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

        public virtual void LinkInputsAndOutputs(List<Module> modules)
        {
            Inputs = modules.Where(x => x.OutputIDS.Contains(ID)).ToList();
            Outputs = modules.Where(x => OutputIDS.Contains(x.ID)).ToList();
            Inputs.ForEach(i => i.InputProcessed += OnInputProcessed);
            InputsLinked?.Invoke(this, EventArgs.Empty);
        }

        private void OnInputProcessed(object? sender, InputProcessedEventArgs e)
        {
            if (e.OutputPulse == Pulse.High)
                HighPulsesSent++;
            if (e.OutputPulse == Pulse.Low)
                LowPulsesSent++;
        }

        protected abstract Pulse? Process(string inputID, Pulse inputPulse);
        public List<(string inputID, Module output, Pulse pulse)> ProcessInput(string inputID, Pulse inputPulse)
        {
            //Console.WriteLine($"{inputID} -{inputPulse}-> {ID}");
            var outputPulse = Process(inputID, inputPulse);
            var outgoingPulses = new List<(string inputID, Module output, Pulse pulse)>();
            if (outputPulse.HasValue)
            {
                outgoingPulses.AddRange(Outputs.Select(o => (ID, o, outputPulse.Value)));
            }

            InputProcessed?.Invoke(this, new InputProcessedEventArgs
            {
                Module = this,
                InputPulse = inputPulse,
                OutputPulse = outputPulse
            });
            return outgoingPulses;
        }
    }

    private class InputProcessedEventArgs : EventArgs
    {
        public Module Module { get; set; }
        public Pulse InputPulse { get; set; }
        public Pulse? OutputPulse { get; set; }
    }
    private enum Pulse
    {
        High,
        Low,
    }
}