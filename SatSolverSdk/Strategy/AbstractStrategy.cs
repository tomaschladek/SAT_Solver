using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract BitArray Solve(SatDefinitionDto definition);
        public virtual IEnumerable<(BitArray Fenotyp, long Score)> Execute(SatDefinitionDto definition)
        {
            var bitArray = Solve(definition);
            yield return (bitArray, ScoreComputation.GetClearScores(definition, bitArray, Cache).Score - definition.Clauses.Count);
        }

        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
        protected IDictionary<int, FormulaResultDto> Cache = new Dictionary<int, FormulaResultDto>();
    }
}