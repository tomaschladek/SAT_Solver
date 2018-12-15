using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract FenotypDto Solve(SatDefinitionDto definition);
        public virtual IEnumerable<FenotypDto> Execute(SatDefinitionDto definition)
        {
            var fenotyp = Solve(definition);
            yield return fenotyp;
        }

        public abstract string Id { get; }

        protected readonly SatScoreComputations ScoreComputation = new SatScoreComputations();
        protected virtual IDictionary<int, FormulaResultDto> Cache => null;
    }
}