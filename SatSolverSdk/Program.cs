using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Services;
using SatSolverSdk.Strategy;
using SatSolverSdk.Strategy.GeneticAlgorithm;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;
using SatSolverSdk.Strategy.GeneticAlgorithm.Crossing;
using SatSolverSdk.Strategy.GeneticAlgorithm.Selections;

namespace SatSolverSdk
{
    class Program
    {
        private static IReadSatManager _reader;
        private static IExecutor _executor;

        static void Main()
        {
            Initialize();
            var mode = EMode.GeneticAlgorithm;
            var definitions = GetInputs(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\50_218_SAT\Weighted", 100).ToList();
            switch (mode)
            {
                case EMode.Generation:
                    new InstanceGenerator().Generate(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\50_218_SAT", "Weighted");
                    break;
                case EMode.Execution:
                    Execute(new DpllStrategy(),definitions);
                    break;
                case EMode.GeneticAlgorithm:
                    Execute(new GeneticStrategy(500, 500, 3, 90, new RandomCrossStrategy(), 
                        new TournamentSelectionStrategy(5, 5, 0, new NoCorrectionStrategy()), true), definitions);
                    break;
            }
        }

        private static void Initialize()
        {
            _reader = new ReadSatManager();
            _executor = new Executor();
        }

        private static void Execute(IStrategy strategy, List<SatDefinitionDto> definitions)
        {
            var duration = _executor.ExecuteOverDefinitions(strategy, definitions, @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\PAA\SAT\enlarged50_218_SAT.csv");
            Console.WriteLine($"Duration: {duration}");
            Console.WriteLine($"=====================================");
            //Console.ReadLine();
        }

        private static IEnumerable<SatDefinitionDto> GetInputs(string folder, int takeCount)
        {
            foreach (var file in Directory.GetFiles(folder).Take(takeCount))
            {
                yield return _reader.ReadDefinition(file);
            }
        }

        private enum EMode
        {
            Generation,Execution,GeneticAlgorithm
        }
    }
}
