using System.Collections;
using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract BitArray Solve(SatDefinitionDto definition);
        public virtual IEnumerable<(long, BitArray)> Execute(SatDefinitionDto definition)
        {
            yield return (1, Solve(definition));
        }

        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
    }
}