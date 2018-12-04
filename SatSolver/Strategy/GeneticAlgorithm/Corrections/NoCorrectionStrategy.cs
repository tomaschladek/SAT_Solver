using System.Collections;
using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Strategy.GeneticAlgorithm.Corrections
{
    public class NoCorrectionStrategy : ICorrectionStrategy
    {
        public string Id => "No correction";

        public IEnumerable<BitArray> CorrectGeneration(SatDefinitionDto definition, List<BitArray> generation)
        {
            return generation;
        }
    }
}