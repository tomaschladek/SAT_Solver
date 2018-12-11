using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Services;
using SatSolver.Strategy;
using SatSolver.Strategy.GeneticAlgorithm;
using SatSolver.Strategy.GeneticAlgorithm.Corrections;
using SatSolver.Strategy.GeneticAlgorithm.Crossing;
using SatSolver.Strategy.GeneticAlgorithm.Selections;

namespace SatSolver
{
    class Program
    {
        private static IReadSatManager _reader;
        private static IExecutor _executor;

        static void Main()
        {
            Initialize();
            var mode = EMode.GeneticAlgorithm;
            switch (mode)
            {
                case EMode.Generation:
                    new InstanceGenerator().Generate(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\", "Weighted");
                    break;
                case EMode.Execution:
                    Execute(new DpllStrategy());
                    break;
                case EMode.GeneticAlgorithm:
                    //Execute(new GeneticStrategy(300, 100, 20, 90, new RandomCrossStrategy(),
                    //    new TournamentSelectionStrategy(30, 30, 0, new NoCorrectionStrategy())));
                    //Execute(new GeneticStrategy(50, 500, 2, 90, new RandomCrossStrategy(), 
                    //    new FitnessSelectionStrategy(15, 0, new NoCorrectionStrategy())));
                    //Execute(new GeneticStrategy(50, 1000, 2, 90, new RandomCrossStrategy(), 
                    //    new FitnessSelectionStrategy(15, 0, new NoCorrectionStrategy())));
                    //Execute(new GeneticStrategy(300, 100, 2, 90, new RandomCrossStrategy(), 
                    //    new TournamentSelectionStrategy(5, 5, 0, new NoCorrectionStrategy())));
                    //Execute(new GeneticStrategy(300, 100, 2, 90, new RandomCrossStrategy(), 
                    //    new TournamentSelectionStrategy(10, 5, 0, new NoCorrectionStrategy())));

                    Execute(new DpllStrategy());
                    Execute(new GeneticStrategy(300, 100, 2, 90, new RandomCrossStrategy(), 
                        new TournamentSelectionStrategy(5, 5, 0, new NoCorrectionStrategy()), true));
                    Execute(new GeneticStrategy(100, 200, 25, 90, new RandomCrossStrategy(), 
                        new TournamentSelectionStrategy(5,5, 0, new NoCorrectionStrategy()), false));
                    break;
            }
        }

        private static void Initialize()
        {
            _reader = new ReadSatManager();
            _executor = new Executor();
        }

        private static void Execute(IStrategy strategy)
        {
            var definitions = GetInputs(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\20_91_SAT\Weighted", 100).ToList();
            var duration = _executor.ExecuteOverDefinitions(strategy, definitions, @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\PAA\SAT\Quality_20_91_SAT.csv");
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
