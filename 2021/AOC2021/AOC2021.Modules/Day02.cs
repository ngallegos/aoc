using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2021.Modules
{
    public class Day02
    {
        
        private IEnumerable<string> get_input(){
            
            var assembly = this.GetType().GetTypeInfo().Assembly;
            using (var s = assembly.GetManifestResourceStream("AOC2021.Modules.Inputs.day-02-01.txt"))
            {
                using (var sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        yield return line;
                }
            }
        }
        public (int xPosition, int yPosition) Part1()
        {
            var commands = get_input().Select(x => new SubmarineCommand(x)).ToArray();

            var currentX = 0;
            var currentY = 0;

            foreach (var command in commands)
            {
                (currentX, currentY) = command.NextPosition01(currentX, currentY);
            }
            
            return (currentX, currentY);
        }

        public (int horizontalPosition, int depth, int aim) Part2()
        {
            var commands = get_input().Select(x => new SubmarineCommand(x)).ToArray();

            var currentX = 0;
            var currentY = 0;
            var currentAim = 0;

            foreach (var command in commands)
            {
                (currentX, currentY, currentAim) = command.NextPositionWithAim(currentX, currentY, currentAim);
            }
            
            return (currentX, currentY, currentAim);
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