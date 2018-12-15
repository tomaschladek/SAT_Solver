using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;

namespace SatSolverSdk.Strategy
{
    public class DpllStrategy : AbstractStrategy
    {
        public override FenotypDto Solve(SatDefinitionDto definition)
        {
            return ScoreComputation.GetBest(Execute(definition).ToList());
        }

        public override IEnumerable<FenotypDto> Execute(SatDefinitionDto definition)
        {
            var emptySolution = new BitArray(definition.VariableCount, true);
            var presence = new BitArray(definition.VariableCount, false);
            SetPureVariable(definition, emptySolution, presence);
            return Solve(definition, emptySolution, presence)
                .Select(item => ScoreComputation.GetClearScores(definition,item,null));
        }

        private IEnumerable<BitArray> Solve(SatDefinitionDto definition, BitArray solution, BitArray presence)
        {
            var nextPosition = GetNextPosition(presence);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                yield return solution;
                yield break;
            }

            var result = UnitClausePropagation(definition, solution, presence);
            if (!result)
            {
                yield break;
            }

            nextPosition = GetNextPosition(presence);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                yield return solution;
                yield break;
            }


            var valuesWith = FindSolution(definition, solution, presence, nextPosition.Value, true);
            foreach (var value in valuesWith)
            {
                yield return value;
            }

            var valuesWithout = FindSolution(definition, solution, presence, nextPosition.Value, false);
            foreach (var value in valuesWithout)
            {
                yield return value;
            }
        }

        private bool UnitClausePropagation(SatDefinitionDto definition, BitArray solution, BitArray presence)
        {
            bool hasChanged;
            do
            {
                hasChanged = false;
                foreach (var clause in definition.Clauses)
                {
                    var variables = clause.Variables.Select(item => new VariableDto(item)).ToList();
                    if (variables.All(variable =>
                        presence[variable.Index] && solution[variable.Index] != variable.IsPositive))
                    {
                        return false;
                    }
                    if (variables.Any(variable =>
                        presence[variable.Index] && solution[variable.Index] == variable.IsPositive))
                    {
                        continue;
                    }
                    var undecided = variables.Where(variable => !presence[variable.Index]).ToList();
                    if (undecided.Count == 1)
                    {
                        var variable = undecided.Single();
                        solution[variable.Index] = variable.IsPositive;
                        presence[variable.Index] = true;
                        hasChanged = true;
                    }
                }
            } while (hasChanged);

            return true;
        }

        private void SetPureVariable(SatDefinitionDto definition, BitArray solution, BitArray presence)
        {
            var exceptSet = new HashSet<int>();
            var refernces = new BitArray(definition.VariableCount);
            var referncesSet = new BitArray(definition.VariableCount);
            foreach (var clause in definition.Clauses)
            {
                foreach (var variable in clause.Variables.Select(item => new VariableDto(item)))
                {
                    if (!referncesSet[variable.Index])
                    {
                        refernces[variable.Index] = variable.IsPositive;
                        referncesSet[variable.Index] = true;
                    }
                    else if (refernces[variable.Index] != variable.IsPositive)
                    {
                        exceptSet.Add(variable.Index);
                    }
                }
            }

            for (int index = 0; index < definition.VariableCount; index++)
            {
                if (exceptSet.Contains(index) || !referncesSet[index])
                {
                    continue;
                }

                solution[index] = refernces[index];
                presence[index] = true;
            }
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

        private IEnumerable<BitArray> FindSolution(SatDefinitionDto definition, BitArray solution, BitArray presence, int nextPosition, bool nextValue)
        {
            var nextWithTrue = new BitArray(solution)
            {
                [nextPosition] = nextValue
            };
            var newPresence = new BitArray(presence)
            {
                [nextPosition] = true
            };
            var isSatisfiableWith = ScoreComputation.IsSatisfiable(definition, nextWithTrue, newPresence, Cache);
            if (isSatisfiableWith.Satisfaction == ESatisfaction.All)
            {
                // All are already satisfied
                yield return nextWithTrue;
            }
            if (isSatisfiableWith.Satisfaction == ESatisfaction.Some)
            {
                // Evaluation is partial and no conflicting clauses found 
                var solutionWith = Solve(definition, nextWithTrue, newPresence);
                foreach (var value in solutionWith)
                {
                    yield return value;
                }
            }
        }

        public override string Id => "DPLL";
    }
}