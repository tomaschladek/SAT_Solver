using System;
using System.Collections.Generic;
using System.Linq;
using SatSolver.Dtos;

namespace SatSolver.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract IList<bool> Solve(SatDefinitionDto definition);
        public abstract string Id { get; }


        protected bool? IsSatisfiable(IList<bool?> partialSolution, ClausesDto clause)
        {
            bool? isAnyVariableSatisfied = false;
            foreach (var variable in clause.Variables.Select(item => new VariableDto(item)))
            {

                if (!partialSolution[variable.Index].HasValue)
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

        protected ResultDto IsSatisfiable(SatDefinitionDto definition, IList<bool?> partialSolution)
        {
            var isAnyFailed = false;
            var counter = 0;
            var areAllClausesSatisfied = true;
            foreach (var clause in definition.Clauses)
            {
                bool? isAnyVariableSatisfied = IsSatisfiable(partialSolution, clause);

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

            return new ResultDto(counter, isAnyFailed ? ESatisfaction.NotSatisfiedExists : areAllClausesSatisfied
                ? ESatisfaction.All
                : ESatisfaction.Some);
        }

        protected static List<bool> GetResult(IList<bool?> solution)
        {
            return solution.Select(item => item ?? false).ToList();
        }

        protected class VariableDto
        {
            public int Index { get; set; }
            public bool IsPositive { get; set; }

            public VariableDto(int index, bool isPositive)
            {
                Index = index;
                IsPositive = isPositive;
            }

            public VariableDto(int variable)
            {
                Index = Math.Abs(variable) - 1;
                IsPositive = variable > 0;
            }
        }

        public class ResultDto
        {
            public int Counter { get; set; }
            public ESatisfaction Satisfaction { get; set; }

            public ResultDto(int counter, ESatisfaction satisfaction)
            {
                Counter = counter;
                Satisfaction = satisfaction;
            }
        }


        public enum ESatisfaction
        {
            All, Some, NotSatisfiedExists
        }
    }
}