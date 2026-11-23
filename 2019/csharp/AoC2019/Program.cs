using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AoC2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionsType = typeof(IDaySolutions);
            var solutionTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => solutionsType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .OrderBy(x => x.Name);

            foreach (var solutionType in solutionTypes)
            {
                var solution = (IDaySolutions)Activator.CreateInstance(solutionType);
                var output01 = solution.Solution01();
                var output02 = solution.Solution02();

                Console.WriteLine($"Day {solution.Day}:\n\tSolution 01: {output01}\n\tSolution 02: {output02}");
            }

            Console.ReadLine();
        }

    }
}
