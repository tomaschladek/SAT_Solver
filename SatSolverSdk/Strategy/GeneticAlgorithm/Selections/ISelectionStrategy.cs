using System;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public interface ISelectionStrategy
    {
        IList<FenotypDto> Select(SatDefinitionDto definition, Random random, List<FenotypDto> generation, IDictionary<int, FormulaResultDto> cache);
        string Id { get; }
    }
}