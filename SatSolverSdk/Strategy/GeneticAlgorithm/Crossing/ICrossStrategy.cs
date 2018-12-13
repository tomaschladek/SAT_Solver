using System;
using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Crossing
{
    public interface ICrossStrategy
    {
        IEnumerable<BitArray> Cross(int vectorSize, Random random,
            List<FenotypDto> generation, int populationSize, int crossoverProbability);

        string Id { get; }
    }
}