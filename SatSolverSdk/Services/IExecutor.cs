using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy;

namespace SatSolverSdk.Services
{
    public interface IExecutor
    {
        double Execute(IStrategy strategy, IList<SatDefinitionDto> definitions, string fullPath);
        double ExecuteOverDefinitions(IStrategy strategy, IList<SatDefinitionDto> definitions, string fullPath);
    }

    public class Executor : IExecutor
    {
        private IReadSatManager _provider = new ReadSatManager();
        public double Execute(IStrategy strategy, IList<SatDefinitionDto> definitions, string fullPath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var definition in definitions)
            {
                for (int repetition = 1; repetition <= 10; repetition++)
                {
                    var series = strategy.Execute(definition).ToList();
                    series.ForEach(item => item.Score -= definition.VariableCount);
                    _provider.AppendFile(fullPath, new []{ $"{definition.FileName}-Run{repetition}" }.Concat(series.Select(item => item.Score.ToString())).ToArray());
                }
            }
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public double ExecuteOverDefinitions(IStrategy strategy, IList<SatDefinitionDto> definitions, string fullPath)
        {
            
            var cache = new List<long>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var definition in definitions)
            {
                for (int repetition = 1; repetition <= 1; repetition++)
                {
                    var score = strategy.Solve(definition).Score;
                    score -= definition.Clauses.Count;
                    cache.Add(score);
                }
            }
            _provider.AppendFile(fullPath, new[] { $"{strategy.Id}" }.Concat(cache.Select(item => item.ToString())).ToArray());
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public static string PrintSolution(BitArray solution)
        {
            return solution == null
                ? "No solution found!"
                : string.Join(" ", Enumerable.Range(0, solution.Count).Select(index => solution[index] ? index + 1 : -(index + 1)));
        }

    }
}