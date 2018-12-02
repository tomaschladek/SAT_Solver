using System;
using System.Collections.Generic;
using System.Linq;

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

        public override IList<bool> Solve(SatDefinitionDto definition)
        {
            Random generator = new Random();
            for (int probe = 0; probe < MaxProbes; probe++)
            {
                var solution = CreateRandomSolution(definition, generator);
                for (int flip = 0; flip < definition.VariableCount / 2; flip++)
                {

                    if (IsSatisfiable(definition, solution).Satisfaction == ESatisfaction.All)
                    {
                        return solution.Select(item => item ?? false).ToList();
                    }

                    solution = FlipVariableWithMostSatisfiedClauses(definition, solution);
                }
            }

            return null;
        }

        private static IList<bool?> CreateRandomSolution(SatDefinitionDto definition, Random generator)
        {
            var solution = new List<bool?>();
            for (int variableIndex = 0; variableIndex < definition.VariableCount; variableIndex++)
            {
                solution.Add(generator.Next(1, 100) > 50);
            }

            return solution;
        }

        private IList<bool?> FlipVariableWithMostSatisfiedClauses(SatDefinitionDto definition, IList<bool?> solution)
        {
            var max = new {Counter=-1, Solution = default(IList<bool?>)};

            for (int flipIndex = 0; flipIndex < definition.VariableCount; flipIndex++)
            {
                IList<bool?> flipped = new List<bool?>(solution)
                {
                    [flipIndex] = !solution[flipIndex]
                };

                var satisfiedClauses = IsSatisfiable(definition, flipped);
                if (satisfiedClauses.Counter > max.Counter)
                {
                    max = new {satisfiedClauses.Counter, Solution = flipped};
                }
            }

            return max.Solution;
        }
    }
}