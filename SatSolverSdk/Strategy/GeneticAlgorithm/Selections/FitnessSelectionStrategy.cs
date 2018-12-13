using System;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public class FitnessSelectionStrategy : AbstractSelectionStrategy
    {
        public override string Id => $"Fitness-E:{ElitesCount}-W:{WeakestsCount}-Corr:{CorrectionStrategy.Id}";

        public FitnessSelectionStrategy() : this(0, 0, new NoCorrectionStrategy())
        {
        }

        public FitnessSelectionStrategy(int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy) : base(elitesCount, weakestsCount, correctionStrategy)
        {
        }

        protected override IEnumerable<FenotypDto> SelectByCriteria(SatDefinitionDto definition, Random random,
            List<FenotypDto> generation, IDictionary<int, FormulaResultDto> cache)
        {
            var sumScore = generation.Sum(item => item.Score);
            for (var newGenerationIndex = StartCount; newGenerationIndex < generation.Count; newGenerationIndex++)
            {
                var randomValue = random.Next(0, (int)sumScore);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += generation[counter].Score;
                } while (sumValue <= randomValue);

                yield return generation[counter];
            }
        }
    }
}