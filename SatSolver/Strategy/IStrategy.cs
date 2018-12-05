using System.Collections;
using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public interface IStrategy
    {
        BitArray Solve(SatDefinitionDto definition);

        IEnumerable<(long, BitArray)> Execute(SatDefinitionDto definition);

        string Id { get; }
    }
}