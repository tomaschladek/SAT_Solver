using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Corrections
{
    public interface ICorrectionStrategy
    {
        IEnumerable<FenotypDto> CorrectGeneration(SatDefinitionDto definition, List<FenotypDto> generation);
        string Id { get; }
    }
}