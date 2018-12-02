using NUnit.Framework;
using SatSolver.Strategy;

namespace SatSolverUnitTests.Strategies
{
    [TestFixture]
    public sealed class DpllStrategyUnitTest2Should : AbstractStrategyUnitTest
    {
        private IStrategy Strategy => new DpllStrategy();

        [Test]
        [TestCase(1, true, false, false, true, false, true, false, false, false, true, false, false, true, true, true,
            false, true, false, false, true)]
        [TestCase(2, true, false, false, false, true, true, true, true, true, false, false, true, false, true, true,
            true, false, false, true, false)]
        [TestCase(3, true, true, true, true, false, true, true, true, true, true, true, false, true, false, false, true,
            true, true, false, true)]
        [TestCase(4, true, false, true, true, false, false, true, false, false, true, true, false, true, false, false,
            true, true, false, false, false)]
        [TestCase(5, false, false, false, false, true, false, true, false, false, true, false, true, true, false, true,
            true, false, true, false, true)]
        [TestCase(6, true, true, false, false, true, false, false, true, false, true, true, true, false, true, true,
            true, true, true, true, true)]
        [TestCase(7, false, false, true, true, false, true, true, true, true, false, false, true, true, false, true,
            false, false, true, true, true)]
        [TestCase(8, false, false, true, true, false, true, false, false, true, false, true, false, true, false, false,
            false, false, false, false, true)]
        [TestCase(9, false, true, true, false, false, true, false, true, false, true, true, true, true, false, true,
            true, true, false, false, false)]
        [TestCase(10, true, false, true, true, true, true, false, true, true, false, false, true, true, true, true,
            false, false, false, true, true)]
        public void ReturnExpectedValues(int index, bool v1, bool v2, bool v3, bool v4, bool v5,
            bool v6, bool v7, bool v8, bool v9, bool v10,
            bool v11, bool v12, bool v13, bool v14, bool v15,
            bool v16, bool v17, bool v18, bool v19, bool v20)
        {
            var definition = ReadManager.ReadDefinition(
                $@"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\UMI\Sat\uf20-0{index}.cnf");
            var solution = Strategy.Solve(definition);
            Assert.AreEqual(v1, solution[0]);
            Assert.AreEqual(v2, solution[1]);
            Assert.AreEqual(v3, solution[2]);
            Assert.AreEqual(v4, solution[3]);
            Assert.AreEqual(v5, solution[4]);
            Assert.AreEqual(v6, solution[5]);
            Assert.AreEqual(v7, solution[6]);
            Assert.AreEqual(v8, solution[7]);
            Assert.AreEqual(v9, solution[8]);
            Assert.AreEqual(v10, solution[9]);
            Assert.AreEqual(v11, solution[10]);
            Assert.AreEqual(v12, solution[11]);
            Assert.AreEqual(v13, solution[12]);
            Assert.AreEqual(v14, solution[13]);
            Assert.AreEqual(v15, solution[14]);
            Assert.AreEqual(v16, solution[15]);
            Assert.AreEqual(v17, solution[16]);
            Assert.AreEqual(v18, solution[17]);
            Assert.AreEqual(v19, solution[18]);
            Assert.AreEqual(v20, solution[19]);
        }
    }
}