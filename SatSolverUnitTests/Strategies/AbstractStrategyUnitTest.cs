using NUnit.Framework;
using SatSolver;

namespace SatSolverUnitTests.Strategies
{
    [TestFixture]
    public abstract class AbstractStrategyUnitTest
    {
        protected ReadSatManager ReadManager;

        [SetUp]
        public void Setup()
        {
            ReadManager = new ReadSatManager();
        }
    }
}