using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public abstract class AbstractSelectionStrategy : ISelectionStrategy
    {
        protected SatScoreComputations ScoreComputation = new SatScoreComputations();
        public AbstractSelectionStrategy(int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy)
        {
            ElitesCount = elitesCount;
            WeakestsCount = weakestsCount;
            CorrectionStrategy = correctionStrategy;
        }

        protected ICorrectionStrategy CorrectionStrategy { get; set; }

        protected int ElitesCount { get; set; }
        protected int WeakestsCount { get; set; }
        public int StartCount { get; set; }

        private IEnumerable<BitArray> SelectElites(SatDefinitionDto definition, List<BitArray> generation,
            IDictionary<BitArray, FormulaResultDto> cache)
        {
            var elites = ScoreComputation
                .GetScores(definition, generation,cache)
                .OrderByDescending(tuple => tuple.Item2)
                .Take(ElitesCount).ToArray();

            StartCount = elites.Length - WeakestsCount;

            return elites.Select(item => item.item.item);
        }

        private IEnumerable<BitArray> RemoveWeakests(SatDefinitionDto definition, List<BitArray> generation,
            IDictionary<BitArray, FormulaResultDto> cache)
        {
            return ScoreComputation
                .GetScores(definition, generation, cache)
                .OrderBy(tuple => tuple.Item2)
                .Skip(WeakestsCount)
                .Select(item => item.item.item);
        }


        public IEnumerable<BitArray> Select(SatDefinitionDto definition, Random random, List<BitArray> generation, IDictionary<BitArray, FormulaResultDto> cache)
        {
            var correctedGeneration = CorrectionStrategy.CorrectGeneration(definition, generation).ToList();
            var elites = SelectElites(definition, correctedGeneration, cache).ToList();
            var childrenByScore = SelectByCriteria(definition, random, generation, cache);
            //var result = RemoveWeakests(definition, elites.Concat(childrenByScore).ToList());
            return elites.Concat(childrenByScore);
        }

        public abstract string Id { get; }

        protected abstract IEnumerable<BitArray> SelectByCriteria(SatDefinitionDto definition, Random random,
            List<BitArray> generation, IDictionary<BitArray, FormulaResultDto> cache);
    }
}