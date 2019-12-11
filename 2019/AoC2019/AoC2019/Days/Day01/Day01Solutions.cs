using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Days.Day01
{
    public class Day01Solutions : DaySolutionsBase<Day01Inputs>
    {
        public override int Day => 1;
        protected override string SolvePuzzle01(List<string> input)
        {
            var fuels = new List<int>();
            foreach (var mass in input.Select(Int32.Parse))
            {
                var fuelNeeded = GetFuelForMass(mass);
                fuels.Add(fuelNeeded);
            }

            return fuels.Sum().ToString();
        }

        protected override string SolvePuzzle02(List<string> input)
        {
            var fuels = new List<int>();
            foreach (var mass in input.Select(Int32.Parse))
            {
                var fuelNeeded = GetFuelForMass(mass);
                var fuelForFuelNeeded = GetFuelForFuel(fuelNeeded) - fuelNeeded;
                fuels.Add(fuelNeeded);
                fuels.Add(fuelForFuelNeeded);
            }

            return fuels.Sum().ToString();
        }

        private int GetFuelForMass(int mass)
        {
            return (int) (Math.Floor((decimal) mass / 3M) - 2);
        }

        private int GetFuelForFuel(int fuel)
        {
            var fuelsFuel = GetFuelForMass(fuel);
            if (fuelsFuel <= 0)
                return fuel;
            return fuel + GetFuelForFuel(fuelsFuel);
        }
    }
}
