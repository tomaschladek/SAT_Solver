﻿using NUnit.Framework;
using SatSolverSdk.Strategy;

namespace SatSolverSdkUnitTests.Strategies
{
    [TestFixture]
    public sealed class BackTrackingStrategyUnitTest2Should : AbstractStrategyUnitTest
    {
        private IStrategy Strategy => new BacktrackingStrategy();

        [Test]
        public void FindSolution([Range(1, 10)] int index)
        {
            var definition = ReadManager.ReadDefinition(
                $@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\20_91_SAT\uf20-0{index}.cnf");
            var solution = Strategy.Solve(definition);
            Assert.NotNull(solution);
        }
    }
}