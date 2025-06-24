using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegerDivisionUnitTestProject
{
    using Math;

    [TestClass]
    public class IntegerDivisionTest
    {
        [TestMethod]
        public void TestDivideSame()
        {
            IntegerDivision One = new IntegerDivision(11, 11);
            Assert.IsTrue(One.Quotient == 1L && One.Reminder == 0L, string.Format("Quotient: {0}. Reminder: {1}", One.Quotient, One.Reminder));
        }

        [TestMethod]
        public void TestDivideDouble()
        {
            IntegerDivision One = new IntegerDivision(120, 60);
            Assert.IsTrue(One.Quotient == 2L && One.Reminder == 0L, string.Format("Quotient: {0}. Reminder: {1}", One.Quotient, One.Reminder));
        }

        [TestMethod]
        public void TestDivideSmaller()
        {
            IntegerDivision One = new IntegerDivision(120, 127);
            Assert.IsTrue(One.Quotient == 0L && One.Reminder == 120L, string.Format("Quotient: {0}. Reminder: {1}", One.Quotient, One.Reminder));
        }

        [TestMethod]
        public void TestDivide()
        {
            long numerator = 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23 + 1;
            long divisor = 39;
            long expectedQuotient = 5 * 7 * 11 * 17 * 19 * 23;
            long expectedReminder = 1;

            IntegerDivision One = new IntegerDivision(numerator, divisor);
            Assert.IsTrue(One.Quotient == expectedQuotient && One.Reminder == expectedReminder, string.Format("Quotient: {0}. Reminder: {1}", One.Quotient, One.Reminder));
        }
    }
}
