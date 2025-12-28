using AOC.Helpers;
using Google.OrTools.LinearSolver;

namespace AOC2025.Tests;

public class Day10Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var machineDefinitions = get_sample(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.ComputeFewestButtonPresses());
        
        // Assert
        shortestPresses.Sum().ShouldBe(7);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var machineDefinitions = get_input(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.ComputeFewestButtonPresses());
        
        // Assert
        shortestPresses.Sum().ShouldBe(520);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var machineDefinitions = get_sample(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.GetValidVoltageCombination());
        
        // Assert
        shortestPresses.Sum().ShouldBe(33);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var machineDefinitions = get_input(x => new Machine(x)).ToList();
        
        // Act
        var shortestPresses = machineDefinitions.Select(m => m.GetValidVoltageCombination());
        
        // Assert
        shortestPresses.Sum().ShouldBe(20626);
    }

    class Machine
    {
        public Machine(string definition)
        {
            var parts = definition.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            LightDiagram = parts[0].Trim('[', ']');
            LightValue = Convert.ToInt64(LightDiagram.Replace('#', '1').Replace('.', '0'), 2);
            WiringSchematics = parts[1..^1].Select(x => x.Trim('(', ')').Split(',').Select(int.Parse).ToArray()).ToArray();
            WiringSchematicsValues = new long[WiringSchematics.Length];
            for (var i = 0; i < WiringSchematics.Length; i++)
            {
                var emptyDiagram = new int[LightDiagram.Length];
                foreach (var index in WiringSchematics[i])
                {
                    emptyDiagram[index] = 1;
                }
                WiringSchematicsValues[i] = Convert.ToInt64(string.Join("",emptyDiagram.Select(x => x.ToString())), 2);
            }
            JoltageRequirements = parts[^1].Trim('{', '}').Split(',').Select(int.Parse).ToArray();
        }

        public int ComputeFewestButtonPresses()
        {
            var path = Search<long>.BFS(0L, 
                x => WiringSchematicsValues.Select(s => x ^ s).ToList(), 
                x => x == LightValue);

            return (path?.Count ?? 0) - 1;
        }
        
        // https://www.reddit.com/r/adventofcode/comments/1pity70/2025_day_10_solutions/
        // https://developers.google.com/optimization/lp/lp_example
        public int GetValidVoltageCombination()
        {
            var solver = Solver.CreateSolver("SCIP");
            var objective = solver.Objective();
            objective.SetMinimization();
            var buttonVariables = new List<Variable>();
            for (int bVar = 0; bVar < WiringSchematics.Length; bVar++)
            {
                var bVariable = solver.MakeIntVar(0, 1000, $"b{bVar}");
                buttonVariables.Add(bVariable);
                objective.SetCoefficient(bVariable, 1);
            }
            for (int joltageIndex = 0; joltageIndex < JoltageRequirements.Length; joltageIndex++)
            {
                var expectedJoltage = JoltageRequirements[joltageIndex];
                var releventButtonVariables = new List<Variable>();
                for (int buttonIndex = 0; buttonIndex < WiringSchematics.Length; buttonIndex++)
                {
                    if (WiringSchematics[buttonIndex].Contains(joltageIndex))
                    {
                        releventButtonVariables.Add(buttonVariables[buttonIndex]);
                    }
                }
                var expr = new LinearExpr();
                foreach (var bVar in releventButtonVariables)
                    expr += bVar;
                var constraint = expr == expectedJoltage;
                solver.Add(constraint);
            }
            
            solver.Solve();
            var totalSum = (int)buttonVariables.Sum(bVar => bVar.SolutionValue());
            // Console.WriteLine($"solution = {totalSum}: {string.Join(", ", buttonVariables.Select(bv =>
            //     $"{bv.Name()}={bv.SolutionValue()}"
            // ))}");
            return totalSum;

        }

        long LightValue { get; }
        string LightDiagram { get; }
        int[][] WiringSchematics { get; }
        long[] WiringSchematicsValues { get; }
        int[] JoltageRequirements { get; }
    }
}
