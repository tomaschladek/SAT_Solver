using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Corrections
{
    public interface ICorrectionStrategy
    {
        IEnumerable<BitArray> CorrectGeneration(SatDefinitionDto definition, List<BitArray> generation);
        string Id { get; }
    }
}