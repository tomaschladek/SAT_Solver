using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract BitArray Solve(SatDefinitionDto definition);
        public virtual IEnumerable<(long, BitArray)> Execute(SatDefinitionDto definition)
        {
            var bitArray = Solve(definition);
            yield return (ScoreComputation.GetClearScores(definition, bitArray, Cache).Item2 - definition.Clauses.Count, bitArray);
        }

        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
        protected IDictionary<BitArray,FormulaResultDto> Cache = new Dictionary<BitArray, FormulaResultDto>();
    }
}