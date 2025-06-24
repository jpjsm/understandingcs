using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyDive_WorkingWithArrays;

namespace SkiDiverUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Skydive dive = new Skydive(1.0, 0.1, 0.581, 1.035, 80);
            double[] actualVelocity = dive.GetVelocityseries();
            Assert.IsTrue((0.981- actualVelocity[1]) < 0.0001,"Actual velocity at t1 different than expected");
            Assert.IsTrue((1.9616 - actualVelocity[2]) < 0.0001, "Actual velocity at t2 different than expected");
            Assert.IsTrue((2.9409 - actualVelocity[3]) < 0.0001, "Actual velocity at t3 different than expected");
            Assert.IsTrue((3.9182 - actualVelocity[4]) < 0.0001, "Actual velocity at t4 different than expected");
            Assert.IsTrue((8.7457 - actualVelocity[9]) < 0.0001, "Actual velocity at t9 different than expected");

        }
    }
}
