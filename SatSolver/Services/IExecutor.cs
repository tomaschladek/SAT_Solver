using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SatSolver.Dtos;
using SatSolver.Strategy;

namespace SatSolver.Services
{
    public interface IExecutor
    {
        double Execute(IStrategy strategy, IList<SatDefinitionDto> definitions, string fullPath);
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
                    _provider.AppendFile(fullPath, new []{ $"{definition.FileName}-Run{repetition}" }.Concat(series.Select(item => item.Item1.ToString())).ToArray());
                }
            }
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public static string PrintSolution(BitArray solution)
        {
            return solution == null
                ? "No solution found!"
                : string.Join(' ', Enumerable.Range(0, solution.Count).Select(index => solution[index] ? index + 1 : -(index + 1)));
        }

    }
}