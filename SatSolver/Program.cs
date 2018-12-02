using System;
using System.Collections;
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
            var definitions = GetInputs().ToList();
            var strategy = new DpllStrategy();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int repetition = 0; repetition < 50; repetition++)
            {
                foreach (var definition in definitions)
                {
                    strategy.Solve(definition);
                }
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

        public static string PrintSolution(BitArray solution)
        {
            return solution == null
                            ? "No solution found!"
                            : string.Join(' ', Enumerable.Range(0,solution.Count).Select(index => solution[index] ? index + 1 : -(index + 1)));
        }
    }
}
