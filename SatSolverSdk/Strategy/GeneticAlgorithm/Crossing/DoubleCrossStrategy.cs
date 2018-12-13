using System;
using System.Collections;
using System.Collections.Generic;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Crossing
{
    public class DoubleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(int vectorSize, Random random, List<FenotypDto> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count - 1; fenotypIndex += 2)
            {
                var cutFrom = random.Next(0, vectorSize);
                var cutTo = random.Next(0, vectorSize);
                var first = new BitArray(generation[fenotypIndex].Fenotyp);
                var second = new BitArray(generation[fenotypIndex + 1].Fenotyp);

                var counter = cutFrom;
                while (counter != cutTo)
                {
                    var temp = generation[fenotypIndex].Fenotyp[counter];
                    first[counter] = second[counter];
                    second[counter] = temp;
                    counter = (counter + 1) % first.Count;
                }

                yield return first;
                yield return second;
            }
        }

        public string Id => "2 point cross";
    }
}