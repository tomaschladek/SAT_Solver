using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using SatSolver;
using SatSolver.Dtos;
using SatSolver.Services;

namespace SatSolverUnitTests
{
    [TestFixture]
    public class ReadSatManagerUnitTest
    {
        private ReadSatManager ReadSatManager { get; set; }

        [SetUp]
        public void Setup()
        {
            ReadSatManager = new ReadSatManager();
        }

        [TestFixture]
        public sealed class ReadSatManagerUnitTestShould : ReadSatManagerUnitTest
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

        [TestFixture]
        public sealed class ReadSatManagerMockUnitTestShould : ReadSatManagerUnitTest
        {
            private Mock<IIoProvider> _ioProvider;
            private Mock<StreamWriter> _writer;
            private StringBuilder _builder;

            [SetUp]
            public void SetUp()
            {
                _ioProvider = new Mock<IIoProvider>();
                ReadSatManager.IoProvider = _ioProvider.Object;
                _builder = new StringBuilder();
                _writer = new Mock<StreamWriter>("asasdfsdaf");
                _writer.Setup(foo => foo.WriteLine(It.IsAny<string>())).Callback<string>(text => _builder.AppendLine(text));
                _ioProvider.Setup(foo => foo.GetFileWrite(It.IsAny<string>())).Returns(_writer.Object);
            }
            [Test]
            public void ParseCorrectDefinition()
            {
                var definition = new SatDefinitionDto(5,3);
                definition.Weights = new List<int>{1,2,3,4,5};
                definition.Clauses.Add(new ClausesDto(new List<int>{1,-2,3}));
                definition.Clauses.Add(new ClausesDto(new List<int>{2,-3,4}));
                definition.Clauses.Add(new ClausesDto(new List<int>{3,-4,5}));

                ReadSatManager.WriteDefinition(definition,string.Empty);

                var text = _builder.ToString().Split(Environment.NewLine,StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual(5,text.Length);

                Assert.AreEqual("p cnf 5 3", text[0]);
                Assert.AreEqual("w 1 2 3 4 5", text[1]);
                Assert.AreEqual("1 -2 3 0", text[2]);
                Assert.AreEqual("2 -3 4 0", text[3]);
                Assert.AreEqual("3 -4 5 0", text[4]);
            }

        }
    }
}