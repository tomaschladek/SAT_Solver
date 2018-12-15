using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class SatScoreComputations
    {
        public FenotypDto GetBest(List<FenotypDto> generation)
        {
            if (!generation.Any())
            {
                return null;
            }
            var maxWeight = generation.Max(item => item.Score);
            var result = generation.First(item => item.Score == maxWeight);
            return new FenotypDto(result.Fenotyp, maxWeight, result.SatResult);
        }

        public IEnumerable<FenotypDto> GetScores(SatDefinitionDto definition,
            List<BitArray> generation, IDictionary<int, FormulaResultDto> cache)
        {
            var presence = new BitArray(definition.VariableCount, true);

            return generation
                .Select(item =>
                {
                    var satResult = IsSatisfiable(definition, item, presence, cache);
                    var score = satResult.Weights + satResult.Counter;
                    return new FenotypDto(item, score, satResult);
                });
        }

        public FenotypDto GetClearScores(SatDefinitionDto definition,
            BitArray generation, IDictionary<int, FormulaResultDto> cache)
        {
            return GetScores(definition,new List<BitArray> { generation}, cache)
                .Single();
        }

        public long GetWeights(BitArray fenotyp, SatDefinitionDto definition)
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
            int hash = 1;
            if (cache != null)
            {
                hash = GetHashCode(partialSolution);
                if (cache.ContainsKey(hash))
                {
                    return cache[hash];
                }
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

            var satisfaction = isAnyFailed ? ESatisfaction.NotSatisfiedExists : areAllClausesSatisfied
                ? ESatisfaction.All
                : ESatisfaction.Some;
            var weight = satisfaction == ESatisfaction.All
                ? GetWeights(partialSolution, definition)
                : 0;
            var result = new FormulaResultDto(counter, satisfaction,weight);
            cache?.Add(hash, result);
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