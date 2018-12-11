using NUnit.Framework;
using SatSolverSdk.Dtos;

namespace SatSolverSdkUnitTests.Dtos
{
    [TestFixture]
    public class BitVectorDtoUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestFixture]
        public sealed class BitVectorDtoUnitTestShould : BitVectorDtoUnitTest
        {
            [Test]
            public void ReturnExpectedValue([Values(true,false)] bool isTrue,[Values(1,2,3,4,5,6,7,8,9,10)] int size)
            {
                var vector = new BitVectorDto(1,false);
                for (int index = 0; index < 1; index++)
                {
                    Assert.AreEqual(false, vector[index]);
                }
            }

            [Test]
            public void ReturnSetFlag([Values(7,21,29,31,32,33,61)] int index)
            {
                var vector = new BitVectorDto(62, false)
                {
                    [index] = true
                };

                vector[index] = true;
                Assert.AreEqual(false, vector[0]);
                Assert.AreEqual(false, vector[1]);
                Assert.AreEqual(false, vector[62]);
                Assert.AreEqual(true, vector[index]);
            }

            [Test]
            public void CreateDuplicate()
            {
                var vector = new BitVectorDto(62, false)
                {
                    [7] = true
                };

                var copy = new BitVectorDto(vector);

                vector[8] = true;
                copy[9] = true;

                Assert.IsTrue(vector[7]);
                Assert.IsTrue(vector[8]);

                Assert.IsTrue(copy[7]);
                Assert.IsTrue(copy[9]);

                Assert.IsFalse(copy[8]);
                Assert.IsFalse(vector[9]);
            }
        }
    }
}