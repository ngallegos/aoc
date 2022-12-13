using System.Linq;

namespace AOC2015.Modules
{
    public class Day01 : DayBase
    {
        public override bool Completed => true;

        public override dynamic Part1()
        {
            var elevatorInstructions = get_input().First();
            var floor = 0;
            foreach (var instruction in elevatorInstructions)
            {
                if (instruction == '(')
                    floor++;
                else if (instruction == ')')
                    floor--;
            }
            
            return new { floor };
        }

        public override dynamic Part2()
        {
            var elevatorInstructions = get_input().First();
            var floor = 0;
            var basementEnteredAtPosition = 0;
            foreach (var item in elevatorInstructions.Select((x, i) => new { instruction = x, i }))
            {
                if (item.instruction == '(')
                    floor++;
                else if (item.instruction == ')')
                    floor--;
                if (floor < 0)
                {
                    basementEnteredAtPosition = item.i + 1;
                    break;
                }
            }
            
            return new { basementEnteredAtPosition };
        }
    }
}