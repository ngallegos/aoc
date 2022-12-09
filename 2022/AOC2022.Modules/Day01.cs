using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace AOC2022.Modules
{
    public class Day01 : DayBase
    {
        public override bool Completed => true;

        public override dynamic Part1()
        {
            var calorieList = get_input().ToList();
            var orderedElfCalories = GetElvesByCaloriesCarried(calorieList);
            
            return new { mostCaloriesCarried = orderedElfCalories.First() };
        }

        public override dynamic Part2()
        {
            var calorieList = get_input().ToList();
            var orderedElfCalories = GetElvesByCaloriesCarried(calorieList);
            
            return new { topThreeCalorieCarriersTotal = orderedElfCalories.Take(3).Sum() };
        }

        private List<int> GetElvesByCaloriesCarried(List<string> calorieList)
        {
            var elfCalories = new List<int>();
            var currentElfCalories = 0;
            foreach (var calorieEntry in calorieList)
            {
                if (int.TryParse(calorieEntry.Trim(), out int calories))
                    currentElfCalories += calories;
                else
                {
                    elfCalories.Add(currentElfCalories);
                    currentElfCalories = 0;
                }
            }

            return elfCalories.OrderByDescending(x => x).ToList();
        }
    }
}