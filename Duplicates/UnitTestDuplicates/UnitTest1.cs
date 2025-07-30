using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDuplicates
{
    using Duplicates;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConstructor()
        {
            Duplicates dups = new Duplicates(@"c:\tmp");
        }
    }
}
