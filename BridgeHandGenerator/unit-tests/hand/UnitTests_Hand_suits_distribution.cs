using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BridgeHandGenerator
{
    public class UnitTests_Hand_suits_distribution
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            Hand_suits_distribution shape = new Hand_suits_distribution();

            int max = 13;
            int min = 0;
            Assert.AreEqual(max, shape.Spades.Max);
            Assert.AreEqual(min, shape.Spades.Min);

            Assert.AreEqual(max, shape.Hearts.Max);
            Assert.AreEqual(min, shape.Hearts.Min);

            Assert.AreEqual(max, shape.Diamonds.Max);
            Assert.AreEqual(min, shape.Diamonds.Min);

            Assert.AreEqual(max, shape.Clubs.Max);
            Assert.AreEqual(min, shape.Clubs.Min);            
        }

        [Test]
        public void TestListConstructor()
        {
            Suit_length suit_5_6 = new Suit_length(5, 6);
            Suit_length suit_null_null = new Suit_length(null, null);

            Hand_suits_distribution spades_5_6 = new Hand_suits_distribution([(Suits.Spades, new Suit_length(5,6))]);

            Assert.AreEqual(suit_5_6, spades_5_6.Spades);
            Assert.AreEqual(suit_null_null, spades_5_6.Hearts);
            Assert.AreEqual(suit_null_null, spades_5_6.Diamonds);
            Assert.AreEqual(suit_null_null, spades_5_6.Clubs);
        }

        [Test]
        public void TestProperConstructor()
        {
            Suit_length spades = new Suit_length(4, 4);
            Suit_length hearts = new Suit_length(3, 3);
            Suit_length diamonds = new Suit_length(3, 3);
            Suit_length clubs = new Suit_length(3, 3);
            Hand_suits_distribution shape = new Hand_suits_distribution(spades, hearts, diamonds, clubs);
            Assert.AreEqual(spades, shape.Spades);
            Assert.AreEqual(hearts, shape.Hearts);
            Assert.AreEqual(diamonds, shape.Diamonds);
            Assert.AreEqual(clubs, shape.Clubs);
        }

        [Test]
        public void TestConstructorWithSuitAllNull()
        {
            Suit_length suit_null_null = new Suit_length(null, null);
            Suit_length suit_3_3 = new Suit_length(3, 3);

            // Spades is null
            Hand_suits_distribution shape = new Hand_suits_distribution(suit_null_null, suit_3_3, suit_3_3, suit_3_3);
            Assert.AreEqual(suit_null_null, shape.Spades);
            Assert.AreEqual(suit_3_3, shape.Hearts);
            Assert.AreEqual(suit_3_3, shape.Diamonds);
            Assert.AreEqual(suit_3_3, shape.Clubs);

            // Hearts is null
            shape = new Hand_suits_distribution(suit_3_3, suit_null_null, suit_3_3, suit_3_3);
            Assert.AreEqual(suit_3_3, shape.Spades);
            Assert.AreEqual(suit_null_null, shape.Hearts);
            Assert.AreEqual(suit_3_3, shape.Diamonds);
            Assert.AreEqual(suit_3_3, shape.Clubs);

            // Diamonds is null
            shape = new Hand_suits_distribution(suit_3_3, suit_3_3, suit_null_null, suit_3_3);
            Assert.AreEqual(suit_3_3, shape.Spades);
            Assert.AreEqual(suit_3_3, shape.Hearts);
            Assert.AreEqual(suit_null_null, shape.Diamonds);
            Assert.AreEqual(suit_3_3, shape.Clubs);

            // Clubs is null
            shape = new Hand_suits_distribution(suit_3_3, suit_3_3, suit_3_3, suit_null_null);
            Assert.AreEqual(suit_3_3, shape.Spades);
            Assert.AreEqual(suit_3_3, shape.Hearts);
            Assert.AreEqual(suit_3_3, shape.Diamonds);
            Assert.AreEqual(suit_null_null, shape.Clubs);
        }

        [Test]
        public void TestConstructorExceptions()
        {
            // Minimums exceed total
            Assert.Throws<ApplicationException>(() => new Hand_suits_distribution(
                new Suit_length(4, 4),
                new Suit_length(4, 4),
                new Suit_length(4, 4),
                new Suit_length(4, 4)
            ));

            // Maximums less than total
            Assert.Throws<ApplicationException>(() => new Hand_suits_distribution(
                new Suit_length(0, 0),
                new Suit_length(0, 0),
                new Suit_length(0, 0),
                new Suit_length(0, 0)
            ));
        }

    }
}