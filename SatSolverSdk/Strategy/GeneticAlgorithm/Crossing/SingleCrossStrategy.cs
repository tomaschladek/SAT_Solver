using System;
using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Crossing
{
    public class SingleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(int vectorSize, Random random, List<FenotypDto> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count; fenotypIndex += 2)
            {
                var cut = random.Next(0, vectorSize);
                var first = new BitArray(generation[fenotypIndex].Fenotyp);
                var second = new BitArray(generation[fenotypIndex + 1].Fenotyp);
                for (int index = cut; index < first.Count; index++)
                {
                    var temp = generation[fenotypIndex].Fenotyp[index];
                    first[index] = second[index];
                    second[index] = temp;
                }

                yield return first;
                yield return second;
            }
        }

        public string Id => "1 point cross";
    }
}