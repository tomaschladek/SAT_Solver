using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Strategy.GeneticAlgorithm;

namespace SatSolver.Strategy
{
    public class SatScoreComputations
    {
        public (long, BitArray) GetBest(SatDefinitionDto definition, List<BitArray> generation)
        {
            var candidates = GetBests(definition, generation).ToList();
            var maxWeight = candidates.Max(item => item.Item2);
            var result = candidates.First(item => item.Item2 == maxWeight);
            return (maxWeight, result.Item1.Item1);
        }

        private IEnumerable<((BitArray item, FormulaResultDto) item, long)> GetBests(SatDefinitionDto definition, List<BitArray> generation)
        {
            var scoredFormulas = GetScores(definition, generation).ToList();
            var maxSatisfiedClauses = scoredFormulas.Max(item => item.Item1.Item2.Counter);
            return scoredFormulas
                .Where(item => item.Item1.Item2.Counter == maxSatisfiedClauses);
        }

        public IEnumerable<((BitArray item, FormulaResultDto) item, long)> GetScores(SatDefinitionDto definition, List<BitArray> generation)
        {
            var presence = new BitArray(definition.VariableCount, true);
            return generation
                .Select(item => (item, IsSatisfiable(definition, item, presence)))
                .Select(item => (item,
                    item.Item2.Satisfaction == ESatisfaction.All
                        ? GetScoreItem(item.Item1, definition)
                        : item.Item2.Counter - definition.Clauses.Count));
        }

        public long GetScoreItem(BitArray fenotyp, SatDefinitionDto definition)
        {
            var weight = 0L;
            for (int index = 0; index < fenotyp.Count; index++)
            {
                if (fenotyp[index])
                {
                    weight += definition.Weights[index];
                }
            }
            return weight;
        }

        public bool? IsSatisfiable(BitArray partialSolution, BitArray presence, ClausesDto clause)
        {
            bool? isAnyVariableSatisfied = false;
            foreach (var variable in clause.Variables.Select(item => new VariableDto(item)))
            {

                if (!presence[variable.Index])
                {
                    isAnyVariableSatisfied = null;
                    continue;
                }

                if (partialSolution[variable.Index] == variable.IsPositive)
                {
                    isAnyVariableSatisfied = true;
                    break;
                }
            }

            return isAnyVariableSatisfied;
        }

        public FormulaResultDto IsSatisfiable(SatDefinitionDto definition, BitArray partialSolution, BitArray presence)
        {
            var isAnyFailed = false;
            var counter = 0;
            var areAllClausesSatisfied = true;
            foreach (var clause in definition.Clauses)
            {
                bool? isAnyVariableSatisfied = IsSatisfiable(partialSolution, presence, clause);

                if (isAnyVariableSatisfied == false)
                {
                    isAnyFailed = true;
                }
                else if (isAnyVariableSatisfied == null)
                {
                    areAllClausesSatisfied = false;
                }
                else
                {
                    counter++;
                }
            }

            return new FormulaResultDto(counter, isAnyFailed ? ESatisfaction.NotSatisfiedExists : areAllClausesSatisfied
                ? ESatisfaction.All
                : ESatisfaction.Some);
        }
    }
}