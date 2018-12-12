using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class SatScoreComputations
    {
        public (BitArray Fenotyp, long Score) GetBest(SatDefinitionDto definition, List<BitArray> generation,
            IDictionary<int, FormulaResultDto> cache)
        {
            var candidates = GetBests(definition, generation, cache).ToList();
            var maxWeight = candidates.Max(item => item.Score);
            var result = candidates.First(item => item.Score == maxWeight);
            return (result.Fenotyp, maxWeight);
        }

        private IEnumerable<(BitArray Fenotyp, FormulaResultDto SatResult, long Score)> GetBests(SatDefinitionDto definition,
            List<BitArray> generation, IDictionary<int, FormulaResultDto> cache)
        {
            var scoredFormulas = GetScores(definition, generation, cache).ToList();
            var maxSatisfiedClauses = scoredFormulas.Max(item => item.SatResult.Counter);
            return scoredFormulas
                .Where(item => item.SatResult.Counter == maxSatisfiedClauses);
        }

        public IEnumerable<(BitArray Fenotyp, FormulaResultDto SatResult, long Score)> GetScores(SatDefinitionDto definition,
            List<BitArray> generation, IDictionary<int, FormulaResultDto> cache)
        {
            var presence = new BitArray(definition.VariableCount, true);

            return generation
                .Select(item =>
                {
                    var satResult = IsSatisfiable(definition, item, presence, cache);
                    var score = satResult.Satisfaction == ESatisfaction.All
                        ? GetScoreItem(item, definition) + definition.Clauses.Count
                        : satResult.Counter;
                    return (item, satResult,score);
                });
        }

        public (BitArray Fenotyp, FormulaResultDto SatResult, long Score) GetClearScores(SatDefinitionDto definition,
            BitArray generation, IDictionary<int, FormulaResultDto> cache)
        {
            return GetScores(definition,new List<BitArray> { generation}, cache)
                .Single();
        }

        public long GetScoreItem(BitArray fenotyp, SatDefinitionDto definition)
        {
            var index = 0;
            return definition.Weights.Sum(item => fenotyp[index++] ? item : 0);
        }

        public bool? IsSatisfiable(BitArray partialSolution, BitArray presence, ClausesDto clause)
        {
            bool? isAnyVariableSatisfied = false;
            for (var index = 0; index < clause.Variables.Count; index++)
            {
                var variable = new VariableDto(clause.Variables[index]);
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

        public FormulaResultDto IsSatisfiable(SatDefinitionDto definition, BitArray partialSolution, BitArray presence, IDictionary<int, FormulaResultDto> cache)
        {
            var hash = GetHashCode(partialSolution);
            if (cache.ContainsKey(hash))
            {
                return cache[hash];
            }
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

            var result = new FormulaResultDto(counter, isAnyFailed ? ESatisfaction.NotSatisfiedExists : areAllClausesSatisfied
                ? ESatisfaction.All
                : ESatisfaction.Some);
            cache.Add(hash, result);
            return result;
        }

        private int GetHashCode(BitArray partialSolution)
        {
            var data = GenerateValues(partialSolution);
            int hash = 17;
            foreach (var vector32 in data)
            {
                hash = hash * 23 + vector32.GetHashCode();
            }
            return hash;
        }

        private static int[] GenerateValues(BitArray partialSolution)
        {
            var size = (int) Math.Ceiling((double) partialSolution.Count / 32);
            var result = new int[size];
            partialSolution.CopyTo(result, 0);
            return result;
        }
    }
}