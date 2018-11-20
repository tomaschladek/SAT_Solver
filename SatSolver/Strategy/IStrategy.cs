using System.Collections.Generic;

namespace SatSolver.Strategy
{
    public interface IStrategy
    {
        IList<bool> Solve(SatDefinitionDto definition);

        string Id { get; }
    }
}