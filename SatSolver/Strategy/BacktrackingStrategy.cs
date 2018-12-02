using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public class BacktrackingStrategy : AbstractStrategy
    {
        public override string Id => "BT";

        public override IList<bool> Solve(SatDefinitionDto definition)
        {
            var emptySolution = Enumerable.Repeat((bool?)null,definition.VariableCount).ToList();
            return Solve(definition, emptySolution);
        }

        private IList<bool> Solve(SatDefinitionDto definition, IList<bool?> solution)
        {
            var nextPosition = GetNextPosition(solution);
            if (nextPosition == null)
            {
                // All fields filled - END condition
                return solution.Select(item => item ?? false).ToList();
            }

            return FindSolution(definition, solution, nextPosition.Value, true)
                   ?? FindSolution(definition, solution, nextPosition.Value, false);
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
    }
}