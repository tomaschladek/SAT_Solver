using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Corrections
{
    public class NoCorrectionStrategy : ICorrectionStrategy
    {
        public string Id => "No correction";

        public IEnumerable<FenotypDto> CorrectGeneration(SatDefinitionDto definition, List<FenotypDto> generation)
        {
            return generation;
        }
    }
}