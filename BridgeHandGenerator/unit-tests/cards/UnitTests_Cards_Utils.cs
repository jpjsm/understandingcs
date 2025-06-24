using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BridgeHandGenerator
{
    public class UnitTests_Cards_Utils
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Card_HCP()
        {
            (int expected, Cards actual)[] test_cases = [
                (4, Cards.Clubs_Ace),
                (4, Cards.Diamonds_Ace),
                (4, Cards.Hearts_Ace),
                (4, Cards.Spades_Ace),

                (3, Cards.Clubs_King),
                (3, Cards.Diamonds_King),
                (3, Cards.Hearts_King),
                (3, Cards.Spades_King),

                (2, Cards.Clubs_Queen),
                (2, Cards.Diamonds_Queen),
                (2, Cards.Hearts_Queen),
                (2, Cards.Spades_Queen),

                (1, Cards.Clubs_Jack),
                (1, Cards.Diamonds_Jack),
                (1, Cards.Hearts_Jack),
                (1, Cards.Spades_Jack),

                (0, Cards.Clubs_10),
                (0, Cards.Diamonds_10),
                (0, Cards.Hearts_10),
                (0, Cards.Spades_10),

                (0, Cards.Clubs_9),
                (0, Cards.Diamonds_9),
                (0, Cards.Hearts_9),
                (0, Cards.Spades_9),

                (0, Cards.Clubs_8),
                (0, Cards.Diamonds_8),
                (0, Cards.Hearts_8),
                (0, Cards.Spades_8),

                (0, Cards.Clubs_7),
                (0, Cards.Diamonds_7),
                (0, Cards.Hearts_7),
                (0, Cards.Spades_7),

                (0, Cards.Clubs_6),
                (0, Cards.Diamonds_6),
                (0, Cards.Hearts_6),
                (0, Cards.Spades_6),

                (0, Cards.Clubs_5),
                (0, Cards.Diamonds_5),
                (0, Cards.Hearts_5),
                (0, Cards.Spades_5),

                (0, Cards.Clubs_4),
                (0, Cards.Diamonds_4),
                (0, Cards.Hearts_4),
                (0, Cards.Spades_4),

                (0, Cards.Clubs_3),
                (0, Cards.Diamonds_3),
                (0, Cards.Hearts_3),
                (0, Cards.Spades_3),

                (0, Cards.Clubs_2),
                (0, Cards.Diamonds_2),
                (0, Cards.Hearts_2),
                (0, Cards.Spades_2),
            ];

            foreach (var (expected, actual) in test_cases)
            {
                Assert.AreEqual(expected, actual.Card_HCP());
            }
        }

        [Test]
        public void Test_Card_Suit()
        {
            (Suits expected, Cards actual)[] test_cases = [
                (Suits.Clubs, Cards.Clubs_Ace),
                (Suits.Diamonds, Cards.Diamonds_Ace),
                (Suits.Hearts, Cards.Hearts_Ace),
                (Suits.Spades, Cards.Spades_Ace),

                (Suits.Clubs, Cards.Clubs_King),
                (Suits.Diamonds, Cards.Diamonds_King),
                (Suits.Hearts, Cards.Hearts_King),
                (Suits.Spades, Cards.Spades_King),

                (Suits.Clubs, Cards.Clubs_Queen),
                (Suits.Diamonds, Cards.Diamonds_Queen),
                (Suits.Hearts, Cards.Hearts_Queen),
                (Suits.Spades, Cards.Spades_Queen),

                (Suits.Clubs, Cards.Clubs_Jack),
                (Suits.Diamonds, Cards.Diamonds_Jack),
                (Suits.Hearts, Cards.Hearts_Jack),
                (Suits.Spades, Cards.Spades_Jack),

                (Suits.Clubs, Cards.Clubs_10),
                (Suits.Diamonds, Cards.Diamonds_10),
                (Suits.Hearts, Cards.Hearts_10),
                (Suits.Spades, Cards.Spades_10),

                (Suits.Clubs, Cards.Clubs_9),
                (Suits.Diamonds, Cards.Diamonds_9),
                (Suits.Hearts, Cards.Hearts_9),
                (Suits.Spades, Cards.Spades_9),

                (Suits.Clubs, Cards.Clubs_8),
                (Suits.Diamonds, Cards.Diamonds_8),
                (Suits.Hearts, Cards.Hearts_8),
                (Suits.Spades, Cards.Spades_8),

                (Suits.Clubs, Cards.Clubs_7),
                (Suits.Diamonds, Cards.Diamonds_7),
                (Suits.Hearts, Cards.Hearts_7),
                (Suits.Spades, Cards.Spades_7),

                (Suits.Clubs, Cards.Clubs_6),
                (Suits.Diamonds, Cards.Diamonds_6),
                (Suits.Hearts, Cards.Hearts_6),
                (Suits.Spades, Cards.Spades_6),

                (Suits.Clubs, Cards.Clubs_5),
                (Suits.Diamonds, Cards.Diamonds_5),
                (Suits.Hearts, Cards.Hearts_5),
                (Suits.Spades, Cards.Spades_5),

                (Suits.Clubs, Cards.Clubs_4),
                (Suits.Diamonds, Cards.Diamonds_4),
                (Suits.Hearts, Cards.Hearts_4),
                (Suits.Spades, Cards.Spades_4),

                (Suits.Clubs, Cards.Clubs_3),
                (Suits.Diamonds, Cards.Diamonds_3),
                (Suits.Hearts, Cards.Hearts_3),
                (Suits.Spades, Cards.Spades_3),

                (Suits.Clubs, Cards.Clubs_2),
                (Suits.Diamonds, Cards.Diamonds_2),
                (Suits.Hearts, Cards.Hearts_2),
                (Suits.Spades, Cards.Spades_2),
            ];

            foreach (var (expected, actual) in test_cases)
            {
                Assert.AreEqual(expected, actual.Card_Suit());
            }
        }
    }
}
