using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract BitArray Solve(SatDefinitionDto definition);
        public virtual IEnumerable<FenotypDto> Execute(SatDefinitionDto definition)
        {
            var bitArray = Solve(definition);
            var result = ScoreComputation.GetClearScores(definition, bitArray, Cache);
            yield return new FenotypDto(bitArray, result.Score - definition.Clauses.Count,result.SatResult);
        }

        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
        protected virtual IDictionary<int, FormulaResultDto> Cache => null;
    }
}