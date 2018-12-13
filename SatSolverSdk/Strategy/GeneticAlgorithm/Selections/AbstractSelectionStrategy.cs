using System;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public abstract class AbstractSelectionStrategy : ISelectionStrategy
    {
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

        private IList<FenotypDto> SelectElites(List<FenotypDto> generation)
        {
            var elites = generation
                .OrderByDescending(tuple => tuple.Score)
                .Take(ElitesCount).ToList();

            StartCount = elites.Count - WeakestsCount;

            return elites;
        }

        private IEnumerable<FenotypDto> RemoveWeakests(List<FenotypDto> generation)
        {
            if (WeakestsCount <= 0)
            {
                return generation;
            }
            if (WeakestsCount >= generation.Count)
            {
                return new List<FenotypDto>();
            }
            return generation
                .OrderBy(tuple => tuple.Score)
                .Skip(WeakestsCount);
        }


        public IEnumerable<FenotypDto> Select(SatDefinitionDto definition, Random random, List<FenotypDto> generation,
            IDictionary<int, FormulaResultDto> cache)
        {
            var correctedGeneration = CorrectionStrategy.CorrectGeneration(definition, generation).ToList();
            var elites = SelectElites(correctedGeneration).ToList();
            var childrenByScore = SelectByCriteria(definition, random, generation, cache);
            var result = RemoveWeakests(elites.Concat(childrenByScore).ToList());
            return result;
        }

        public abstract string Id { get; }

        protected abstract IEnumerable<FenotypDto> SelectByCriteria(SatDefinitionDto definition, Random random,
            List<FenotypDto> generation, IDictionary<int, FormulaResultDto> cache);
    }
}