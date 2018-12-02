using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SatSolver;
using SatSolver.Dtos;

namespace SatSolverUnitTests
{
    [TestFixture]
    public class ReadSatManagerUnitTest
    {
        private IReadSatManager ReadSatManager { get; set; }

        [SetUp]
        public void Setup()
        {
            ReadSatManager = new ReadSatManager();
        }

        [TestFixture]
        public sealed class ReadSatManagerUnitTest2Should : ReadSatManagerUnitTest
        {
            private SatDefinitionDto _definition;
            private SatDefinitionDto _definitionWithWeights;

            [SetUp]
            public void SetUp()
            {
                _definition = ReadSatManager.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\example.cnf");
                _definitionWithWeights = ReadSatManager.ReadDefinition(@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\example_weights.cnf");
            }
            [Test]
            public void ParseCorrectDefinition()
            {
                Assert.AreEqual(5, _definition.VariableCount);
                Assert.AreEqual(3, _definition.Clauses.Count);
                Assert.AreEqual(5, _definition.Weights.Count);
            }

            [Test]
            public void ParseCorrectClauses()
            {
                Assert.IsTrue(_definition.Clauses[0].Variables.SequenceEqual(new List<int>{1, -5, 4}));
                Assert.IsTrue(_definition.Clauses[1].Variables.SequenceEqual(new List<int>{-1, 5, 3, 4}));
                Assert.IsTrue(_definition.Clauses[2].Variables.SequenceEqual(new List<int>{-3, -4}));
            }

            [Test]
            public void InitializeNoWeight()
            {
                Assert.IsTrue(_definition.Weights.All(weight => weight == 1));
            }

            [Test]
            public void AssignWeightFromDefinition()
            {
                Assert.IsTrue(_definitionWithWeights.Weights.SequenceEqual(new List<int>{1,1,2,3,1}));
            }
        }
    }
}