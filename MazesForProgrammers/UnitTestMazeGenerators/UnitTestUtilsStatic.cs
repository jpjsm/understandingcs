using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMazeGenerators
{
    using MazeGenerators;

    [TestClass]
    public class UnitTestUtilsStatic
    {
        [TestMethod]
        public void TestCoinFlip()
        {
            const int tosses = 1000000;
            Dictionary<CoinSides, int> flips = new Dictionary<CoinSides, int>();
            flips.Add(CoinSides.Head, 0);
            flips.Add(CoinSides.Tail, 0);

            for (int i = 0; i < tosses; i++)
            {
                flips[UtilsStatic.CoinFlip()] += 1;
            }

            Assert.IsTrue(flips[CoinSides.Head] > 0, string.Format("No Heads generated in a {0:N0} tosses.", tosses));
            Assert.IsTrue(flips[CoinSides.Tail] > 0, string.Format("No Tails generated in a {0:N0} tosses.", tosses));

            Assert.IsTrue(Math.Abs(flips[CoinSides.Head] - flips[CoinSides.Tail]) / (2.0 * tosses) < 0.01);
        }
    }
}
