using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Services;
using SatSolver.Strategy;

namespace SatSolver
{
    class Program
    {
        private static IReadSatManager _reader;

        static void Main(string[] args)
        {
            _reader = new ReadSatManager();
            var mode = EMode.Execution;
            switch (mode)
            {
                case EMode.Generation:
                    new InstanceGenerator().Generate(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\", "Weighted");
                    break;
                case EMode.Execution:
                    Execute(args);
                    break;
            }
        }

        private static void Execute(string[] args)
        {
            var definitions = GetInputs(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\Weighted\", 10).ToList();
            var strategy = new DpllStrategy();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int repetition = 0; repetition < 100; repetition++)
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

        private static IEnumerable<SatDefinitionDto> GetInputs(string folder, int takeCount)
        {
            foreach (var file in Directory.GetFiles(folder).Take(takeCount))
            {
                yield return _reader.ReadDefinition(file);
            }
        }

        public static string PrintSolution(BitArray solution)
        {
            return solution == null
                            ? "No solution found!"
                            : string.Join(' ', Enumerable.Range(0,solution.Count).Select(index => solution[index] ? index + 1 : -(index + 1)));
        }

        private enum EMode
        {
            Generation,Execution
        }
    }
}
