﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class SatScoreComputations
    {
        public (long, BitArray) GetBest(SatDefinitionDto definition, List<BitArray> generation,
            IDictionary<BitArray, FormulaResultDto> cache)
        {
            var candidates = GetBests(definition, generation, cache).ToList();
            var maxWeight = candidates.Max(item => item.Item2);
            var result = candidates.First(item => item.Item2 == maxWeight);
            return (maxWeight, result.Item1.Item1);
        }

        private IEnumerable<((BitArray item, FormulaResultDto) item, long)> GetBests(SatDefinitionDto definition,
            List<BitArray> generation, IDictionary<BitArray, FormulaResultDto> cache)
        {
            var scoredFormulas = GetScores(definition, generation, cache).ToList();
            var maxSatisfiedClauses = scoredFormulas.Max(item => item.Item1.Item2.Counter);
            return scoredFormulas
                .Where(item => item.Item1.Item2.Counter == maxSatisfiedClauses);
        }

        public IEnumerable<((BitArray item, FormulaResultDto) item, long)> GetScores(SatDefinitionDto definition,
            List<BitArray> generation, IDictionary<BitArray, FormulaResultDto> cache)
        {
            var presence = new BitArray(definition.VariableCount, true);

            return generation
                .Select(item => (item, IsSatisfiable(definition, item, presence,cache)))
                .Select(item => (item,
                    item.Item2.Satisfaction == ESatisfaction.All
                        ? GetScoreItem(item.Item1, definition) + definition.Clauses.Count
                        : item.Item2.Counter));
        }

        public ((BitArray item, FormulaResultDto) item, long) GetClearScores(SatDefinitionDto definition,
            BitArray generation, IDictionary<BitArray, FormulaResultDto> cache)
        {
            return GetScores(definition,new List<BitArray> { generation}, cache)
                .Single();
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

        public FormulaResultDto IsSatisfiable(SatDefinitionDto definition, BitArray partialSolution, BitArray presence, IDictionary<BitArray, FormulaResultDto> cache)
        {
            if (cache.ContainsKey(partialSolution))
            {
                return cache[partialSolution];
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
            cache.Add(partialSolution,result);
            return result;
        }
    }
}