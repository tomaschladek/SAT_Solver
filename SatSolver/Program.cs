using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SatSolver.Strategy;

namespace SatSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SAT Solver!");
            var fullPathExample = @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\Example.cnf";
            var strategies = new IStrategy[]
            {
                new DpllStrategy(),
                new WalkSatStrategy(2000),
                new BacktrackingStrategy(),
                new GsatStrategy(1000)
            };
            var definitions = new Dictionary<int, SatDefinitionDto>();
            foreach (var satIndex in Enumerable.Range(1, 9))
            {
                var fullPath =
                    $@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-0{satIndex}.cnf";
                var reader = new ReadSatManager();
                definitions.Add(satIndex,reader.ReadDefinition(fullPath));
            }


            foreach (var strategy in strategies)
            {
                ExecuteSat(0,strategy, new ReadSatManager().ReadDefinition(fullPathExample));
                Console.WriteLine($"--------------------------------------");
                Console.WriteLine($"{strategy.Id}");
                Console.WriteLine($"--------------------------------------");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (var index in Enumerable.Range(1, 9))
                {
                    ExecuteSat(index, strategy, definitions[index]);
                }
                stopwatch.Stop();
                Console.WriteLine($"Duration: {stopwatch.Elapsed.TotalMilliseconds}");
                Console.WriteLine($"=====================================");
            }

            Console.ReadLine();
        }

        private static void ExecuteSat(int satIndex, IStrategy strategy, SatDefinitionDto definition)
        {
            Console.WriteLine($"Definition: {satIndex}");
            var solution = strategy.Solve(definition);

            Console.WriteLine(solution == null
                ? "No solution found!"
                : string.Join(' ', solution.Select((item, index) => item ? index + 1 : -(index + 1))));
            Console.WriteLine();
        }
    }
}
