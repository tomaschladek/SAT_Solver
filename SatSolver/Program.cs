using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Strategy;

namespace SatSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            var definitions = GetInputs();
            var strategy = new DpllStrategy();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var definition in definitions)
            {
                ExecuteSat(strategy, definition);
            }
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed.TotalMilliseconds}");
            Console.WriteLine($"=====================================");

            Console.ReadLine();
        }

        private static IEnumerable<SatDefinitionDto> GetInputs()
        {
            IReadSatManager reader = new ReadSatManager();
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-01.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-02.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-03.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-04.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-05.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-06.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-07.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-08.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-09.cnf");
            yield return reader.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-010.cnf");
        }

        private static void ExecuteSat(IStrategy strategy, SatDefinitionDto definition)
        {
            var solution = strategy.Solve(definition);

            Console.WriteLine(solution == null
                ? "No solution found!"
                : string.Join(' ', solution.Select((item, index) => item ? index + 1 : -(index + 1))));
        }
    }
}
