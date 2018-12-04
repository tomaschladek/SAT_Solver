﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolver.Strategy.GeneticAlgorithm.Selections
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

        protected override IEnumerable<BitArray> SelectByCriteria(SatDefinitionDto definition, Random random, List<BitArray> generation)
        {
            var score = ScoreComputation
                .GetScores(definition, generation)
                .Select(item => (item.item,item.Item2+definition.VariableCount))
                .ToList();
            var sumScore = score.Sum(item => item.Item2);
            for (var newGenerationIndex = StartCount; newGenerationIndex < generation.Count; newGenerationIndex++)
            {
                var randomValue = random.Next(0, (int)sumScore);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += score[counter].Item2;
                } while (sumValue <= randomValue);

                yield return generation[counter];
            }
        }
    }
}