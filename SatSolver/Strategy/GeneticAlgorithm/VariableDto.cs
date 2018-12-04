using System;

namespace SatSolver.Strategy.GeneticAlgorithm
{
    public class VariableDto
    {
        public int Index { get; set; }
        public bool IsPositive { get; set; }

        public VariableDto(int variable)
        {
            Index = Math.Abs(variable) - 1;
            IsPositive = variable > 0;
        }
    }
}