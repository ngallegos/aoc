using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2022.Modules
{
    public class Day02 : DayBase
    {
        public override dynamic Part1()
        {
            var commands = get_input().Select(x => new SubmarineCommand(x)).ToArray();

            var hPos = 0;
            var depth = 0;

            foreach (var command in commands)
            {
                (hPos, depth) = command.NextPosition01(hPos, depth);
            }
            
            return new {hPos, depth};
        }

        public override dynamic Part2()
        {
            var commands = get_input().Select(x => new SubmarineCommand(x)).ToArray();

            var hPos = 0;
            var depth = 0;
            var aim = 0;

            foreach (var command in commands)
            {
                (hPos, depth, aim) = command.NextPositionWithAim(hPos, depth, aim);
            }
            
            return new {hPos, depth, aim};
        }

        public class SubmarineCommand
        {
            public string Direction { get; }
            public int Distance { get; }
            public SubmarineCommand(string input)
            {
                var commandParts = input.Split(' ');
                Direction = commandParts[0];
                Distance = int.Parse(commandParts[1]);
            }

            public (int x, int y) NextPosition01(int currentX, int currentY)
            {
                switch (Direction)
                {
                    case "forward":
                        return (currentX + Distance, currentY);
                    case "backward":
                        return (currentX - Distance, currentY);
                    case "up":
                        return (currentX, currentY - Distance);
                    case "down":
                        return (currentX, currentY + Distance); // Y is depth so increase on down, decrease on up
                }

                throw new NotImplementedException();
            }
            
            
            public (int x, int y, int aim) NextPositionWithAim(int currentH, int currentDepth, int currentAim)
            {
                switch (Direction)
                {
                    case "forward":
                        return (currentH + Distance, currentDepth + currentAim * Distance, currentAim);
                    case "backward":
                        return (currentH - Distance, currentDepth + currentAim * Distance, currentAim);
                    case "up":
                        return (currentH, currentDepth, currentAim - Distance);
                    case "down":
                        return (currentH, currentDepth, currentAim + Distance); // Y is depth so increase on down, decrease on up
                }

                throw new NotImplementedException();
            }
        }
    }
}