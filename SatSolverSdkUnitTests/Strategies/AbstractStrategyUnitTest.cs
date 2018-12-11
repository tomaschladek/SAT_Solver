using NUnit.Framework;
using SatSolverSdk.Services;

namespace SatSolverSdkUnitTests.Strategies
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