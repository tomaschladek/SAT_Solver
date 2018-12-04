using System.Collections;
using SatSolver.Dtos;
using SatSolver.Strategy.GeneticAlgorithm;

namespace SatSolver.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract BitArray Solve(SatDefinitionDto definition);
        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
    }
}