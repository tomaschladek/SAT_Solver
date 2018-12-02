using System.Collections;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public interface IStrategy
    {
        BitArray Solve(SatDefinitionDto definition);

        string Id { get; }
    }
}