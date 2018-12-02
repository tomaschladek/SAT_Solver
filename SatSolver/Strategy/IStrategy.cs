using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public interface IStrategy
    {
        IList<bool> Solve(SatDefinitionDto definition);

        string Id { get; }
    }
}