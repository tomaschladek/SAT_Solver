﻿using System;
using System.Collections;
using SatSolver.Dtos;
using SatSolver.Strategy.GeneticAlgorithm;

namespace SatSolver.Strategy
{
    public class GsatStrategy : AbstractStrategy
    {
        public override string Id => "GSAT";

        public GsatStrategy(int maxProbes)
        {
            MaxProbes = maxProbes;
        }

        private int MaxProbes { get; set; }

        public override BitArray Solve(SatDefinitionDto definition)
        {
            Random generator = new Random();
            var presence = new BitArray(definition.VariableCount, true);
            for (int probe = 0; probe < MaxProbes; probe++)
            {
                var solution = CreateRandomSolution(definition, generator);
                for (int flip = 0; flip < definition.VariableCount / 2; flip++)
                {

                    if (ScoreComputation.IsSatisfiable(definition, solution, presence).Satisfaction == ESatisfaction.All)
                    {
                        return solution;
                    }

                    solution = FlipVariableWithMostSatisfiedClauses(definition, solution);
                }
            }

            return null;
        }

        private static BitArray CreateRandomSolution(SatDefinitionDto definition, Random generator)
        {
            var solution = new BitArray(definition.VariableCount,true);
            for (int variableIndex = 0; variableIndex < definition.VariableCount; variableIndex++)
            {
                solution[variableIndex] = generator.Next(1, 100) > 50;
            }

            return solution;
        }

        private BitArray FlipVariableWithMostSatisfiedClauses(SatDefinitionDto definition, BitArray solution)
        {
            var max = new {Counter=-1, Solution = default(BitArray)};
            var presence = new BitArray(definition.VariableCount, true);

            for (int flipIndex = 0; flipIndex < definition.VariableCount; flipIndex++)
            {
                var flipped = new BitArray(solution)
                {
                    [flipIndex] = !solution[flipIndex]
                };

                var satisfiedClauses = ScoreComputation.IsSatisfiable(definition, flipped, presence);
                if (satisfiedClauses.Counter > max.Counter)
                {
                    max = new {satisfiedClauses.Counter, Solution = flipped};
                }
            }

            return max.Solution;
        }
    }
}