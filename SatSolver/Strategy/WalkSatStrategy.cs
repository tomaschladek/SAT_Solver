using System;
using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public class WalkSatStrategy : AbstractStrategy
    {
        public override string Id => "WALKSAT";

        public WalkSatStrategy(int maxProbes)
        {
            MaxProbes = maxProbes;
            Probability = 52;
        }

        private int MaxProbes { get; set; }
        private int Probability { get; set; }

        public override IList<bool> Solve(SatDefinitionDto definition)
        {
            Random generator = new Random();
            for (int probe = 0; probe < MaxProbes; probe++)
            {
                var solution = CreateRandomSolution(definition, generator);
                for (int flip = 0; flip < definition.VariableCount * 2; flip++)
                {
                    if (IsSatisfiable(definition, solution).Satisfaction == ESatisfaction.All)
                    {
                        return solution.Select(item => item ?? false).ToList();
                    }

                    var selectedFailedClause = GetFailedClauses(definition, generator, solution);

                    solution = generator.Next(1, 100) > Probability
                        ? FlipMostSatisfiableVariable(definition, solution, selectedFailedClause)
                        : FlipRandomVariable(solution, selectedFailedClause, generator.Next(0, selectedFailedClause.Variables.Count));
                }
            }

            return null;
        }

        private ClausesDto GetFailedClauses(SatDefinitionDto definition, Random generator, IList<bool?> solution)
        {
            var failedClauses = definition.Clauses.Where(clause => IsSatisfiable(solution, clause) == false).ToList();
            var selectedFailedClause = failedClauses[generator.Next(0, failedClauses.Count)];
            return selectedFailedClause;
        }

        private static IList<bool?> CreateRandomSolution(SatDefinitionDto definition, Random generator)
        {
            IList<bool?> solution = new List<bool?>();
            for (int variableIndex = 0; variableIndex < definition.VariableCount; variableIndex++)
            {
                solution.Add(generator.Next(1, 100) > 50);
            }

            return solution;
        }

        private IList<bool?> FlipRandomVariable(IList<bool?> solution, ClausesDto selectedClause, int next)
        {
            var targetIndex = new VariableDto(selectedClause.Variables[next]).Index;
            return new List<bool?>(solution) { [targetIndex] = !solution[targetIndex] };
        }

        private IList<bool?> FlipMostSatisfiableVariable(SatDefinitionDto definition, IList<bool?> solution, ClausesDto selectedClause)
        {
            var max = new { Counter = -1, Solution = default(IList<bool?>) };
            foreach (var variable in selectedClause.Variables.Select(item => new VariableDto(item)))
            {
                IList<bool?> flipped = new List<bool?>(solution) { [variable.Index] = !solution[variable.Index] };

                var satisfiedClauses = IsSatisfiable(definition, flipped);
                if (satisfiedClauses.Counter > max.Counter)
                {
                    max = new { satisfiedClauses.Counter, Solution = flipped };
                }
            }

            return max.Solution;
        }
    }
}