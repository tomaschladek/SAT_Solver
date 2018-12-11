using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SatSolver.Dtos;
using SatSolver.Strategy;
using SatSolver.Strategy.GeneticAlgorithm;

namespace SatSolverUnitTests.Strategies
{
    [TestFixture]
    public class SatScoreComputationUnitTest
    {
        protected SatScoreComputations SatScoreComputations { get; set; }

        [SetUp]
        public void Setup()
        {
            SatScoreComputations = new SatScoreComputations();
        }

        [TestFixture]
        public sealed class SatScoreComputationUnitTestShould : SatScoreComputationUnitTest
        {
            private SatDefinitionDto _definition;

            [SetUp]
            public void SetUp()
            {
                _definition = new SatDefinitionDto("", 5, 3)
                {
                    Weights = new List<int> { 1, 2, 3, 4, 5 },
                    Clauses =
                    {
                        new ClausesDto(new List<int> { 1, -2, 3}),
                        new ClausesDto(new List<int> { 2, -3, 4 }),
                        new ClausesDto(new List<int> { 3, -4, 5 })
                    }
                };
            }
            [Test]
            [TestCase(false,false,false,false,false,0,3,ESatisfaction.All)]
            [TestCase(false,false,false,false,true,5,3,ESatisfaction.All)]
            [TestCase(false,false,false,true,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,false,false, true, true, 9,3,ESatisfaction.All)]
            [TestCase(false,false,true,false,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,false,true,false, true, -1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,false,true,true,false,7,3,ESatisfaction.All)]
            [TestCase(false,false, true, true, true, 12,3,ESatisfaction.All)]
            [TestCase(false,true,false,false,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,true,false,false, true, -1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,true,false,true,false,-2,1,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,true,false, true, true, -1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(false,true,true,false,false,5,3,ESatisfaction.All)]
            [TestCase(false,true,true,false, true, 10,3,ESatisfaction.All)]
            [TestCase(false,true,true,true,false, 9,3,ESatisfaction.All)]
            [TestCase(false, true, true, true, true, 14,3,ESatisfaction.All)]
            [TestCase(true, false,false,false,false,1,3,ESatisfaction.All)]
            [TestCase(true,false,false,false, true, 6,3,ESatisfaction.All)]
            [TestCase(true,false,false,true,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(true,false,false, true, true, 10,3,ESatisfaction.All)]
            [TestCase(true,false,true,false,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(true,false,true,false, true, -1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(true,false,true,true,false,8,3,ESatisfaction.All)]
            [TestCase(true,false, true, true, true, 13,3,ESatisfaction.All)]
            [TestCase(true,true,false,false,false,3,3,ESatisfaction.All)]
            [TestCase(true,true,false,false, true, 8,3,ESatisfaction.All)]
            [TestCase(true,true,false,true,false,-1,2,ESatisfaction.NotSatisfiedExists)]
            [TestCase(true,true,false, true, true, 12,3,ESatisfaction.All)]
            [TestCase(true,true,true,false,false,6,3,ESatisfaction.All)]
            [TestCase(true,true,true,false, true, 11,3,ESatisfaction.All)]
            [TestCase(true,true,true,true,false,10,3,ESatisfaction.All)]
            [TestCase(true, true, true, true, true, 15,3,ESatisfaction.All)]
            public void RetunCorrectScores(bool v1, bool v2, bool v3, bool v4, bool v5, int result, int counter, ESatisfaction satisfaction)
            {
                var fenotyp = new BitArray(5)
                {
                    [0] = v1,
                    [1] = v2,
                    [2] = v3,
                    [3] = v4,
                    [4] = v5
                };
                var score = SatScoreComputations.GetScores(_definition, new List<BitArray>{fenotyp}).Single();
                Assert.AreEqual(result + 3, score.Item2);
                Assert.AreEqual(fenotyp,score.Item1.item);
                Assert.AreEqual(counter,score.Item1.Item2.Counter);
                Assert.AreEqual(satisfaction,score.Item1.Item2.Satisfaction);
                if (result >= 0)
                {
                    Assert.AreEqual(result, SatScoreComputations.GetScoreItem(fenotyp,_definition));
                }
            }

            [Test]
            [TestCase(false, false, false, false, false, 0)]
            [TestCase(false, false, false, false, true, 5)]
            [TestCase(false, false, false, true, false, 4)]
            [TestCase(false, false, false, true, true, 9)]
            [TestCase(false, false, true, false, false, 3)]
            [TestCase(false, false, true, false, true, 8)]
            [TestCase(false, false, true, true, false, 7)]
            [TestCase(false, false, true, true, true, 12)]
            [TestCase(false, true, false, false, false, 2)]
            [TestCase(false, true, false, false, true, 7)]
            [TestCase(false, true, false, true, false, 6)]
            [TestCase(false, true, false, true, true, 11)]
            [TestCase(false, true, true, false, false, 5)]
            [TestCase(false, true, true, false, true, 10)]
            [TestCase(false, true, true, true, false, 9)]
            [TestCase(false, true, true, true, true, 14)]
            [TestCase(true, false, false, false, false, 1)]
            [TestCase(true, false, false, false, true, 6)]
            [TestCase(true, false, false, true, false, 5)]
            [TestCase(true, false, false, true, true, 10)]
            [TestCase(true, false, true, false, false, 4)]
            [TestCase(true, false, true, false, true, 9)]
            [TestCase(true, false, true, true, false, 8)]
            [TestCase(true, false, true, true, true, 13)]
            [TestCase(true, true, false, false, false, 3)]
            [TestCase(true, true, false, false, true, 8)]
            [TestCase(true, true, false, true, false, 7)]
            [TestCase(true, true, false, true, true, 12)]
            [TestCase(true, true, true, false, false, 6)]
            [TestCase(true, true, true, false, true, 11)]
            [TestCase(true, true, true, true, false, 10)]
            [TestCase(true, true, true, true, true, 15)]
            public void ReturnCorrectSumWeight(bool v1, bool v2, bool v3, bool v4, bool v5, int result)
            {
                var fenotyp = new BitArray(5)
                {
                    [0] = v1,
                    [1] = v2,
                    [2] = v3,
                    [3] = v4,
                    [4] = v5
                };
                var score = SatScoreComputations.GetScoreItem(fenotyp, _definition);
                Assert.AreEqual(result, score);
            }

            [Test]
            public void RecognizeOnlySatisfyingFormula()
            {
                var generations = new List<BitArray>
                {
                    new BitArray(new[] {false, false, true, false, true}),
                    new BitArray(new[] {false, false, true, false, true}),
                    new BitArray(new[] {true, true, false, true, false}),
                    new BitArray(new[] {true, true, false, false, false}),
                };
                var score = SatScoreComputations.GetBest(_definition,generations);
                Assert.AreEqual(6, score.Item1, "3 clauses + weight[0] == 1 + weight[1] == 2");
                Assert.AreEqual(generations[3], score.Item2);
            }

            [Test]
            public void RecognizeBestSatisfyingFormula()
            {
                var generations = new List<BitArray>
                {
                    new BitArray(new[] {false, false, false, false, false}),
                    new BitArray(new[] {true,false,true,true,false}),
                    new BitArray(new[] {true, true, false, true, false}),
                    new BitArray(new[] {true, true, false, false, false}),
                };
                var score = SatScoreComputations.GetBest(_definition,generations);
                Assert.AreEqual(11, score.Item1, "8 weights + 3 clauses");
                Assert.AreEqual(generations[1], score.Item2);
            }
        }
    }
}