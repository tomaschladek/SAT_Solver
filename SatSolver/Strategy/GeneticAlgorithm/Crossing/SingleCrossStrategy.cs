using System;
using System.Collections;
using System.Collections.Generic;

namespace SatSolver.Strategy.GeneticAlgorithm.Crossing
{
    public class SingleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(int vectorSize, Random random, List<BitArray> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count; fenotypIndex += 2)
            {
                var cut = random.Next(0, vectorSize);
                var first = new BitArray(generation[fenotypIndex]);
                var second = new BitArray(generation[fenotypIndex + 1]);
                for (int index = cut; index < first.Count; index++)
                {
                    var temp = generation[fenotypIndex][index];
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