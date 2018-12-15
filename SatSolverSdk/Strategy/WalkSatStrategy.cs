using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class WalkSatStrategy : AbstractStrategy
    {
        public override string Id => "WALKSAT";
        private IDictionary<int, FormulaResultDto> _cache = new Dictionary<int, FormulaResultDto>();
        protected override IDictionary<int, FormulaResultDto> Cache => _cache;

        public WalkSatStrategy(int maxProbes)
        {
            MaxProbes = maxProbes;
            Probability = 52;
        }

        private int MaxProbes { get; set; }
        private int Probability { get; set; }

        public override FenotypDto Solve(SatDefinitionDto definition)
        {
            _cache = new Dictionary<int, FormulaResultDto>();
            Random generator = new Random();
            var presence = new BitArray(definition.VariableCount, true);
            for (int probe = 0; probe < MaxProbes; probe++)
            {
                var solution = CreateRandomSolution(definition, generator);
                for (int flip = 0; flip < definition.VariableCount * 2; flip++)
                {
                    if (ScoreComputation.IsSatisfiable(definition, solution, presence, Cache).Satisfaction == ESatisfaction.All)
                    {
                        return ScoreComputation.GetClearScores(definition, solution, null);
                    }

                    var selectedFailedClause = GetFailedClauses(definition, generator, solution);

                    solution = generator.Next(1, 100) > Probability
                        ? FlipMostSatisfiableVariable(definition, solution, selectedFailedClause)
                        : FlipRandomVariable(solution, selectedFailedClause, generator.Next(0, selectedFailedClause.Variables.Count));
                }
            }

            return null;
        }

        private ClausesDto GetFailedClauses(SatDefinitionDto definition, Random generator, BitArray solution)
        {
            var presence = new BitArray(definition.VariableCount,true);
            var failedClauses = definition.Clauses.Where(clause => ScoreComputation.IsSatisfiable(solution, presence, clause) == false).ToList();
            var selectedFailedClause = failedClauses[generator.Next(0, failedClauses.Count)];
            return selectedFailedClause;
        }

        private static BitArray CreateRandomSolution(SatDefinitionDto definition, Random generator)
        {
            var solution = new BitArray(definition.VariableCount);
            for (int variableIndex = 0; variableIndex < definition.VariableCount; variableIndex++)
            {
                solution[variableIndex] = generator.Next(1, 100) > 50;
            }

            return solution;
        }

        private BitArray FlipRandomVariable(BitArray solution, ClausesDto selectedClause, int next)
        {
            var targetIndex = new VariableDto(selectedClause.Variables[next]).Index;
            return new BitArray(solution) { [targetIndex] = !solution[targetIndex] };
        }

        private BitArray FlipMostSatisfiableVariable(SatDefinitionDto definition, BitArray solution, ClausesDto selectedClause)
        {
            var max = new { Counter = -1, Solution = default(BitArray) };
            var presence = new BitArray(definition.VariableCount, true);
            foreach (var variable in selectedClause.Variables.Select(item => new VariableDto(item)))
            {
                var flipped = new BitArray(solution) { [variable.Index] = !solution[variable.Index] };

                var satisfiedClauses = ScoreComputation.IsSatisfiable(definition, flipped, presence, Cache);
                if (satisfiedClauses.Counter > max.Counter)
                {
                    max = new { satisfiedClauses.Counter, Solution = flipped };
                }
            }

            return max.Solution;
        }
    }
}