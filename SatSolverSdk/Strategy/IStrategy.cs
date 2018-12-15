using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public interface IStrategy
    {
        FenotypDto Solve(SatDefinitionDto definition);

        IEnumerable<FenotypDto> Execute(SatDefinitionDto definition);

        string Id { get; }
    }
}