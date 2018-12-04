using System.Collections;
using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy.GeneticAlgorithm.Corrections
{
    public interface ICorrectionStrategy
    {
        IEnumerable<BitArray> CorrectGeneration(SatDefinitionDto definition, List<BitArray> generation);
        string Id { get; }
    }
}