using System.Collections.Generic;
using System.Linq;

namespace SatSolver.Strategy
{
    public class DpllStrategy : AbstractStrategy
    {
        public override IList<bool> Solve(SatDefinitionDto definition)
        {
            var emptySolution = Enumerable.Repeat((bool?)null, definition.VariableCount).ToList();
            SetPureVariable(definition, emptySolution);
            return Solve(definition, emptySolution);
        }

        private IList<bool> Solve(SatDefinitionDto definition, IList<bool?> solution)
        {
            var nextPosition = GetNextPosition(solution);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                return GetResult(solution);
            }

            var result = UnitClausePropagation(definition, solution);
            if (!result.Item2)
            {
                return null;
            }

            nextPosition = GetNextPosition(solution);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                return GetResult(solution);
            }


            return FindSolution(definition, solution, nextPosition.Value, true)
                   ?? FindSolution(definition, solution, nextPosition.Value, false);
        }

        private (IList<bool?>,bool) UnitClausePropagation(SatDefinitionDto definition, IList<bool?> solution)
        {
            bool hasChanged;
            do
            {
                hasChanged = false;
                foreach (var clause in definition.Clauses)
                {
                    var variables = clause.Variables.Select(item => new VariableDto(item)).ToList();
                    if (variables.All(variable =>
                        solution[variable.Index].HasValue && solution[variable.Index] != variable.IsPositive))
                    {
                        return (solution, false);
                    }
                    if (variables.Any(variable =>
                        solution[variable.Index].HasValue && solution[variable.Index] == variable.IsPositive))
                    {
                        continue;
                    }
                    var undecided = variables.Where(variable => solution[variable.Index] == null).ToList();
                    if (undecided.Count == 1)
                    {
                        var variable = undecided.Single();
                        solution[variable.Index] = variable.IsPositive;
                        hasChanged = true;
                    }
                }
            } while (hasChanged);

            return (solution,true);
        }

        private void SetPureVariable(SatDefinitionDto definition, IList<bool?> solution)
        {
            var exceptSet = new HashSet<int>();
            var refernces = Enumerable.Repeat((bool?)null, definition.VariableCount).ToList();
            foreach (var clause in definition.Clauses)
            {
                foreach (var variable in clause.Variables.Select(item => new VariableDto(item)))
                {
                    if (!refernces[variable.Index].HasValue)
                    {
                        refernces[variable.Index] = variable.IsPositive;
                    }
                    else if (refernces[variable.Index].Value != variable.IsPositive)
                    {
                        exceptSet.Add(variable.Index);
                    }
                }
            }

            for (int index = 0; index < definition.VariableCount; index++)
            {
                if (exceptSet.Contains(index) || !refernces[index].HasValue)
                {
                    continue;
                }

                solution[index] = refernces[index];
            }
        }

        private int? GetNextPosition(IList<bool?> solution)
        {
            for (int index = 0; index < solution.Count; index++)
            {
                if (!solution[index].HasValue)
                {
                    return index;
                }
            }

            return null;
        }

        private IList<bool> FindSolution(SatDefinitionDto definition, IList<bool?> solution, int nextPosition, bool nextValue)
        {
            var nextWithTrue = new List<bool?>(solution)
            {
                [nextPosition] = nextValue
            };
            var isSatisfiableWith = IsSatisfiable(definition, nextWithTrue);
            if (isSatisfiableWith.Satisfaction == ESatisfaction.All)
            {
                // All are already satisfied
                return nextWithTrue.Select(item => item ?? true).ToList();
            }
            if (isSatisfiableWith.Satisfaction == ESatisfaction.Some)
            {
                // Evaluation is partial and no conflicting clauses found 
                var solutionWith = Solve(definition, nextWithTrue);
                if (solutionWith != null)
                {
                    return solutionWith;
                }
            }

            return null;
        }

        public override string Id => "DPLL";
    }
}