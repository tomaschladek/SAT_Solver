using NUnit.Framework;
using SatSolver.Services;

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