using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMathClass
{
    using PowerAndRoot;

    [TestClass]
    public class UnitTestMath
    {
        [TestMethod]
        public void TestOdd()
        {
            Assert.IsTrue(Math.Odd(-10001L), "Odd failed for odd number");
            Assert.IsFalse(Math.Odd(-12345678L), "Odd failed for even number");
        }

        [TestMethod]
        public void TestEven()
        {
            Assert.IsTrue(Math.Even(-12345678L), "Even failed for even number");
            Assert.IsFalse(Math.Even(-1234567L), "Even failed for odd number");
        }

        [TestMethod]
        public void TestPower()
        {
            long n = -97L;
            long expected = 1L;
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual<long>(Math.Power(n, i), expected);
                expected *= n;
            }
        }

        [TestMethod]
        public void TestRoot()
        {
            Assert.AreEqual<long>(Math.Root(16777215, 5), 27);
            Assert.AreEqual<long>(Math.Root(16777216, 5), 27);
            long[] N = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
            long[] N5 = { 1048576, 1419857, 1889568, 2476099, 3200000, 4084101, 5153632, 6436343, 7962624, 9765625, 11881376, 14348907, 17210368, 20511149, 24300000, 28629151, 33554432 };
            int j = 0;
            for (int i = 1048576; i < 33554432; i++)
            {
                if (i >= N5[j+1])
                {
                    j++;
                }
                Assert.AreEqual<long>(N[j], Math.Root(i, 5));
            }
        }
    }
}
