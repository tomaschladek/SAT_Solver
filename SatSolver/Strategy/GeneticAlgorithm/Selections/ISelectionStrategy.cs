using System;
using System.Collections;
using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy.GeneticAlgorithm.Selections
{
    public interface ISelectionStrategy
    {
        IEnumerable<BitArray> Select(SatDefinitionDto definition, Random random, List<BitArray> generation);
        string Id { get; }
    }
}