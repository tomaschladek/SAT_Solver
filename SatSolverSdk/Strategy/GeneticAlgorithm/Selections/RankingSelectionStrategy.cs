using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public class RankingSelectionStrategy : AbstractSelectionStrategy
    {
        public override string Id => $"Ranking-E:{ElitesCount}-W:{WeakestsCount}-Corr:{CorrectionStrategy.Id}";

        public RankingSelectionStrategy() : this(0, 0, new NoCorrectionStrategy())
        {
        }

        public RankingSelectionStrategy(int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy) : base(elitesCount, weakestsCount, correctionStrategy)
        {
        }

        protected override IEnumerable<BitArray> SelectByCriteria(SatDefinitionDto definition, Random random, List<BitArray> generation)
        {
            var ordered = ScoreComputation
                .GetScores(definition,generation)
                .Select((bits, index) => new { Bits = bits.item.item, OriginalIndex = index, Score = bits.Item2 })
                .OrderBy(item => item.Score)
                .Select((tuple, newIndex) => new { tuple.Bits, tuple.OriginalIndex, tuple.Score, NewIndex = generation.Count - newIndex })
                .ToList();

            var sumOrder = ordered.Sum(item => item.NewIndex);

            for (int newGenerationIndex = StartCount; newGenerationIndex < generation.Count; newGenerationIndex++)
            {
                var randomValue = random.Next(0, sumOrder);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += ordered[counter].NewIndex;
                } while (sumValue <= randomValue);

                yield return generation[ordered[counter].OriginalIndex];
            }
        }
    }
}