﻿using System.Collections;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class BacktrackingStrategy : AbstractStrategy
    {
        public override string Id => "BT";
        
        public override FenotypDto Solve(SatDefinitionDto definition)
        {
            var emptySolution = new BitArray(definition.VariableCount,true);
            var presence = new BitArray(definition.VariableCount);
            var solution = Solve(definition, emptySolution, presence);
            return ScoreComputation.GetClearScores(definition,solution,null);
        }

        private BitArray Solve(SatDefinitionDto definition, BitArray solution, BitArray presence)
        {
            var nextPosition = GetNextPosition(presence);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                return solution;
            }

            return FindSolution(definition, solution, presence, nextPosition.Value, true)
                   ?? FindSolution(definition, solution, presence, nextPosition.Value, false);
        }

        private int? GetNextPosition(BitArray solution)
        {
            for (int index = 0; index < solution.Count; index++)
            {
                if (!solution[index])
                {
                    return index;
                }
            }

            return null;
        }

        private BitArray FindSolution(SatDefinitionDto definition, BitArray solution, BitArray presence, int nextPosition, bool nextValue)
        {
            var nextWithTrue = new BitArray(solution)
            {
                [nextPosition] = nextValue
            };
            var newPresence = new BitArray(presence)
            {
                [nextPosition] = true
            };
            var isSatisfiableWith = ScoreComputation.IsSatisfiable(definition, nextWithTrue, newPresence,Cache);
            if (isSatisfiableWith.Satisfaction == ESatisfaction.All)
            {
                // All are already satisfied
                return nextWithTrue;
            }
            if (isSatisfiableWith.Satisfaction == ESatisfaction.Some)
            {
                // Evaluation is partial and no conflicting clauses found 
                var solutionWith = Solve(definition, nextWithTrue, newPresence);
                if (solutionWith != null)
                {
                    return solutionWith;
                }
            }

            return null;
        }
    }
}