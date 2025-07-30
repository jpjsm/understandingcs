using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BridgeHandGenerator
{
    public class UnitTests_Suit_length
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            int max = Suit_length.MAX_SUIT_LENGTH;
            int min = Suit_length.MIN_SUIT_LENGTH;

            Suit_length suit = new Suit_length();  

            Assert.AreEqual(min, suit.Min);
            Assert.AreEqual(max, suit.Max); 
        }

        [Test]
        public void TestConstructor()
        {
            Suit_length suit = new Suit_length(4, 6);
            Assert.AreEqual(4, suit.Min);
            Assert.AreEqual(6, suit.Max);
        }

        [Test]
        public void TestConstructorWithNullMin()
        {
            int min = Suit_length.MIN_SUIT_LENGTH;

            Suit_length suit = new Suit_length(null, 7);

            Assert.AreEqual(min, suit.Min);
            Assert.AreEqual(7, suit.Max);
        }
        
        [Test]
        public void TestConstructorWithNullMax()
        {
            int max = Suit_length.MAX_SUIT_LENGTH;

            Suit_length suit = new Suit_length(3, null);
            
            Assert.AreEqual(max, suit.Max);
            Assert.AreEqual(3, suit.Min);
        }
        
        [Test]
        public void TestConstructorWithOutOfBoundValues()
        {
            Assert.Throws<ApplicationException>(() => new Suit_length(-1, 5));
            Assert.Throws<ApplicationException>(() => new Suit_length(-1, null));

            Assert.Throws<ApplicationException>(() => new Suit_length(3, 14));
            Assert.Throws<ApplicationException>(() => new Suit_length(null, 14));

            Assert.Throws<ApplicationException>(() => new Suit_length(5, 3));
        }

        [Test]
        public void TestToString()
        {
            Suit_length all_null = new Suit_length();
            string all_null_expected = $"({Suit_length.MIN_SUIT_LENGTH},{Suit_length.MAX_SUIT_LENGTH})";
            Assert.AreEqual(all_null_expected, all_null.ToString());

            Suit_length max_null = new Suit_length(6,null);
            string max_null_expected = $"(6,{Suit_length.MAX_SUIT_LENGTH})";
            Assert.AreEqual(max_null_expected, max_null.ToString());

            Suit_length min_null = new Suit_length(null,6);
            string min_null_expected = $"({Suit_length.MIN_SUIT_LENGTH},6)";
            Assert.AreEqual(min_null_expected, min_null.ToString());

            Suit_length min_max = new Suit_length(5,6);
            string min_max_expected = "(5,6)";
            Assert.AreEqual(min_max_expected, min_max.ToString());
        }

        [Test]
        public void TestHashCode()
        {
            Suit_length all_null = new Suit_length();
            int all_null_hashcode = Suit_length.MAX_SUIT_LENGTH<<4;
            Assert.AreEqual(all_null_hashcode, all_null.GetHashCode());

            Suit_length min_null_max_value = new Suit_length(null, 8);
            int min_null_max_value_hashcode = 8<<4;
            Assert.AreEqual(min_null_max_value_hashcode, min_null_max_value.GetHashCode());

            Suit_length min_value_max_null = new Suit_length(4, null);
            int min_value_max_null_hashcode = (Suit_length.MAX_SUIT_LENGTH<<4) | 4;
            Assert.AreEqual(min_value_max_null_hashcode, min_value_max_null.GetHashCode());

            Suit_length min_value_max_value = new Suit_length(4,7);
            int min_value_max_value_hashcode = (7<<4) | 4;
            Assert.AreEqual(min_value_max_value_hashcode, min_value_max_value.GetHashCode());

        }

        [Test]
        public void TestEquals()
        {
            Suit_length actual = new Suit_length(3,6);
            (int max, int min) something_else = (3,6);
            Assert.IsFalse(actual.Equals(something_else));
            Assert.IsFalse(actual.Equals(null));

            #pragma warning disable CS8602
            Assert.IsTrue(actual.Equals(actual));
            #pragma warning restore CS8602

            Suit_length expected = new Suit_length(3,6);
            Suit_length not_expected = new Suit_length(2,7);
            Assert.IsTrue(actual.Equals(expected));
            Assert.IsFalse(actual.Equals(not_expected));

            Suit_length full_range = new Suit_length(Suit_length.MIN_SUIT_LENGTH,Suit_length.MAX_SUIT_LENGTH);
            Suit_length max_null_min_null = new Suit_length();
            expected = new Suit_length(Suit_length.MIN_SUIT_LENGTH,Suit_length.MAX_SUIT_LENGTH);
            Assert.IsTrue(full_range.Equals(expected));
            Assert.IsTrue(max_null_min_null.Equals(expected));

            Suit_length max_null_min_value = new Suit_length(3,null);
            expected = new Suit_length(3,Suit_length.MAX_SUIT_LENGTH);
            Assert.IsTrue(max_null_min_value.Equals(expected));

            Suit_length max_value_min_null = new Suit_length(null, 7);
            expected = new Suit_length(Suit_length.MIN_SUIT_LENGTH,7);
            Assert.IsTrue(max_value_min_null.Equals(expected));
        }
    }
}
