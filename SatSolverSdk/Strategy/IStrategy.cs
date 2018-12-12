using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public interface IStrategy
    {
        BitArray Solve(SatDefinitionDto definition);

        IEnumerable<(BitArray Fenotyp, long Score)> Execute(SatDefinitionDto definition);

        string Id { get; }
    }
}