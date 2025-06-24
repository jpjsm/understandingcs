using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestTaskScheduler
{
    using STasks;

    [TestClass]
    public class UnitTestTaskScheduling
    {
        [TestMethod]
        public void TestSingleTask()
        {
            List<STask<Char>> singletask = new List<STask<char>>() { new STask<char>('a') };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(singletask);

            Assert.AreEqual(singletask.Count, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < singletask.Count; i++)
            {
                Assert.AreEqual(singletask[i].Id, scheduledtasks[i], "List are different");
            }
        }

        [TestMethod]
        public void TestIndependentTasks()
        {
            List<STask<Char>> independenttasks = new List<STask<char>>() {
                new STask<char>('a'),
                new STask<char>('b'),
                new STask<char>('c'),
                new STask<char>('d'),
                new STask<char>('e'),
                new STask<char>('f')
            };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(independenttasks);

            Assert.AreEqual(independenttasks.Count, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < independenttasks.Count; i++)
            {
                Assert.AreEqual(independenttasks[i].Id, scheduledtasks[i], "List are different");
            }
        }

        [TestMethod]
        public void TestOrderedTaskList()
        {
            List<STask<Char>> orderedttasks = new List<STask<char>>() {
                new STask<char>('a'),
                new STask<char>('b', new List<char>(){ 'a' }),
                new STask<char>('c', new List<char>(){ 'b' }),
                new STask<char>('d', new List<char>(){ 'c' }),
                new STask<char>('e', new List<char>(){ 'd' }),
                new STask<char>('f', new List<char>(){ 'e' }),
            };


            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(orderedttasks);

            Assert.AreEqual(orderedttasks.Count, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < orderedttasks.Count; i++)
            {
                Assert.AreEqual(orderedttasks[i].Id, scheduledtasks[i], "List are different");
            }
        }

        [TestMethod]
        public void TestReverseOrderedTaskList()
        {
            List<STask<Char>> orderedttasks = new List<STask<char>>() {
                new STask<char>('a', new List<char>(){ 'b' }),
                new STask<char>('b', new List<char>(){ 'c' }),
                new STask<char>('c', new List<char>(){ 'd' }),
                new STask<char>('d', new List<char>(){ 'e' }),
                new STask<char>('e', new List<char>(){ 'f' }),
                new STask<char>('f', new List<char>(){ }),
            };

            Char[] expectedtasks = new char[]{ 'f', 'e', 'd', 'c', 'b', 'a' };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(orderedttasks);

            Assert.AreEqual(expectedtasks.Length, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < expectedtasks.Length; i++)
            {
                Assert.AreEqual(expectedtasks[i], scheduledtasks[i], "List values are different");
            }
        }

        [TestMethod]
        public void TestTreeStructuredTasks1()
        {
            List<STask<Char>> tree1ttasks = new List<STask<char>>() {
                new STask<char>('a', new List<char>(){  }),
                new STask<char>('b', new List<char>(){  }),
                new STask<char>('e', new List<char>(){ 'a', 'b' })
            };

            Char[] expectedtasks = new char[] { 'a', 'b', 'e' };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(tree1ttasks);

            Assert.AreEqual(expectedtasks.Length, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < expectedtasks.Length; i++)
            {
                Assert.AreEqual(expectedtasks[i], scheduledtasks[i], "List values are different");
            }
        }

        [TestMethod]
        public void TestTreeStructuredTasks2()
        {
            List<STask<Char>> tree2ttasks = new List<STask<char>>() {
                new STask<char>('a', new List<char>(){  }),
                new STask<char>('b', new List<char>(){  }),
                new STask<char>('c', new List<char>(){  }),
                new STask<char>('d', new List<char>(){  }),
                new STask<char>('e', new List<char>(){ 'a', 'b' }),
                new STask<char>('f', new List<char>(){ 'c', 'd' }),
                new STask<char>('g', new List<char>(){ 'e', 'f' })
            };

            Char[] expectedtasks = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(tree2ttasks);

            Assert.AreEqual(expectedtasks.Length, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < expectedtasks.Length; i++)
            {
                Assert.AreEqual(expectedtasks[i], scheduledtasks[i], "List values are different");
            }
        }

        [TestMethod]
        public void TestDoubleTreeStructuredTasks1()
        {
            List<STask<Char>> doubletree1ttasks = new List<STask<char>>() {
                new STask<char>('a', new List<char>(){  }),
                new STask<char>('b', new List<char>(){  }),
                new STask<char>('c', new List<char>(){  }),
                new STask<char>('d', new List<char>(){  }),
                new STask<char>('e', new List<char>(){ 'a', 'b' }),
                new STask<char>('f', new List<char>(){ 'c', 'd' }),
                new STask<char>('g', new List<char>(){ 'e', 'f' }),
                new STask<char>('h', new List<char>(){ 'g' }),
                new STask<char>('i', new List<char>(){ 'g' }),
                new STask<char>('j', new List<char>(){ 'h' }),
                new STask<char>('k', new List<char>(){ 'h' }),
                new STask<char>('l', new List<char>(){ 'i' }),
                new STask<char>('m', new List<char>(){ 'i' }),
            };

            Char[] expectedtasks = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm' };

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(doubletree1ttasks);

            Assert.AreEqual(expectedtasks.Length, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < expectedtasks.Length; i++)
            {
                Assert.AreEqual(expectedtasks[i], scheduledtasks[i], "List values are different");
            }
        }

        [TestMethod]
        public void TestDoubleTreeStructuredTasks2()
        {
            List<STask<Char>> doubletree2ttasks = new List<STask<char>>() {
                new STask<char>('p', new List<char>(){ 'n', 'o' }),
                new STask<char>('o', new List<char>(){ 'l', 'm' }),
                new STask<char>('n', new List<char>(){ 'j', 'k' }),
                new STask<char>('m', new List<char>(){ 'i' }),
                new STask<char>('l', new List<char>(){ 'i' }),
                new STask<char>('k', new List<char>(){ 'h' }),
                new STask<char>('j', new List<char>(){ 'h' }),
                new STask<char>('i', new List<char>(){ 'g' }),
                new STask<char>('h', new List<char>(){ 'g' }),
                new STask<char>('g', new List<char>(){ 'e', 'f' }),
                new STask<char>('f', new List<char>(){ 'c', 'd' }),
                new STask<char>('e', new List<char>(){ 'a', 'b' }),
                new STask<char>('d', new List<char>(){  }),
                new STask<char>('c', new List<char>(){  }),
                new STask<char>('b', new List<char>(){  }),
                new STask<char>('a', new List<char>(){  })
            };

            Char[] expectedtasks = new char[] { 'd', 'c', 'b', 'a', 'f', 'e', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

            HashSet<Char> expectedtasks0_3 = new HashSet<char>(new []{ 'a', 'b', 'c', 'd' });
            HashSet<Char> expectedtasks4_5 = new HashSet<char>(new[] { 'e', 'f' });
            HashSet<Char> expectedtasks6 = new HashSet<char>(new[] { 'g' });
            HashSet<Char> expectedtasks7_8 = new HashSet<char>(new[] { 'h', 'i' });

            Char[] scheduledtasks = STasks.STask<Char>.TaskScheduling(doubletree2ttasks);

            Assert.AreEqual(expectedtasks.Length, scheduledtasks.Length, "Length differ");

            for (int i = 0; i < expectedtasks.Length; i++)
            {
                Assert.AreEqual(expectedtasks[i], scheduledtasks[i], "List values are different");
            }
        }
    }
}
