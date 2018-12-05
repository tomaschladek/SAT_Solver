﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Strategy.GeneticAlgorithm.Crossing;
using SatSolver.Strategy.GeneticAlgorithm.Selections;

namespace SatSolver.Strategy.GeneticAlgorithm
{
    public class GeneticStrategy : AbstractStrategy
    {
        public GeneticStrategy(int generationCount, int populationSize, int mutationProbability,
            int crossoverProbability, ICrossStrategy crossStrategy, ISelectionStrategy selectionStrategy)
        {
            Generations = generationCount;
            PopulationSize = populationSize;
            MutationProbability = mutationProbability;
            CrossoverProbability = crossoverProbability;
            CrossStrategy = crossStrategy;
            SelectionStrategy = selectionStrategy;
        }

        private ICrossStrategy CrossStrategy { get; set; }
        private ISelectionStrategy SelectionStrategy { get; set; }

        private int PopulationSize { get; }
        public int Generations { get; set; }
        private int MutationProbability { get; }
        private int CrossoverProbability { get; }

        public override BitArray Solve(SatDefinitionDto definition)
        {
            return Execute(definition).Last().Item2;
        }

        public override IEnumerable<(long, BitArray)> Execute(SatDefinitionDto definition)
        {
            var random = new Random();
            var generation = InitializeGeneration(definition.VariableCount, random).ToList();

            for (var generationIndex = 0; generationIndex < Generations; generationIndex++)
            {
                var generationSelection = SelectionStrategy.Select(definition, random, generation).ToList();
                var generationNew = CrossStrategy.Cross(definition.VariableCount, random, generationSelection, PopulationSize, CrossoverProbability).ToList();

                Mutation(random, generationNew);

                generation = generationNew;
                yield return ScoreComputation.GetBest(definition, generation);
            }
        }

        private void Mutation(Random random, List<BitArray> generationNew)
        {
            foreach (var fenotyp in generationNew)
            {
                for (int fenotypIndex = 0; fenotypIndex < fenotyp.Count; fenotypIndex++)
                {
                    if (random.Next(0, 100) < MutationProbability)
                    {
                        fenotyp.Set(fenotypIndex, !fenotyp[fenotypIndex]);
                    }
                }
            }
        }

        private IEnumerable<BitArray> InitializeGeneration(int vectorSize, Random random)
        {
            for (int generationIndex = 0; generationIndex < PopulationSize; generationIndex++)
            {
                yield return GenerateRandomVector(vectorSize, random);
            }
        }

        private static BitArray GenerateRandomVector(int size, Random random)
        {
            var bitArray = new BitArray(size);
            for (int itemIndex = 0; itemIndex < size; itemIndex++)
            {
                bitArray.Set(itemIndex, random.Next(0, 100) > 50);
            }

            return bitArray;
        }

        public override string Id => $"GA-G:{Generations}-P:{PopulationSize}-M:{MutationProbability}-C:{CrossStrategy.Id}-S:{SelectionStrategy.Id}";
    }
}