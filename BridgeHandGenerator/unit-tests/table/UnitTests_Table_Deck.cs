using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace BridgeHandGenerator
{
    public class UnitTests_Table_Deck
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            Deck deck = new Deck();
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            Cards[] cards = deck.GetDeck;

            CollectionAssert.AreEqual(expected_cards, cards);   
        }

        [Test]
        public void TestShuffle()
        {
            Deck deck = new Deck();
            deck.Shuffle();
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            Cards[] cards = deck.GetDeck;

            CollectionAssert.AreNotEqual(expected_cards, cards);
        }

        [Test]
        public void TestDealNoArguments()
        {
            Deck deck = new Deck();

            Hand expected_north = new Hand([
                Cards.Clubs_2, Cards.Clubs_6, Cards.Clubs_10, Cards.Clubs_Ace,
                Cards.Diamonds_5, Cards.Diamonds_9, Cards.Diamonds_King,
                Cards.Hearts_4, Cards.Hearts_8, Cards.Hearts_Queen,
                Cards.Spades_3, Cards.Spades_7, Cards.Spades_Jack]);
            Hand expected_east = new Hand([
                Cards.Clubs_3, Cards.Clubs_7, Cards.Clubs_Jack,
                Cards.Diamonds_2, Cards.Diamonds_6, Cards.Diamonds_10, Cards.Diamonds_Ace,
                Cards.Hearts_5, Cards.Hearts_9, Cards.Hearts_King,
                Cards.Spades_4, Cards.Spades_8, Cards.Spades_Queen]);
            Hand expected_south = new Hand([
                Cards.Clubs_4, Cards.Clubs_8, Cards.Clubs_Queen,
                Cards.Diamonds_3, Cards.Diamonds_7, Cards.Diamonds_Jack,
                Cards.Hearts_2, Cards.Hearts_6, Cards.Hearts_10, Cards.Hearts_Ace,
                Cards.Spades_5, Cards.Spades_9, Cards.Spades_King]);
            Hand expected_west = new Hand([
                Cards.Clubs_5, Cards.Clubs_9, Cards.Clubs_King,
                Cards.Diamonds_4, Cards.Diamonds_8, Cards.Diamonds_Queen,
                Cards.Hearts_3, Cards.Hearts_7, Cards.Hearts_Jack,
                Cards.Spades_2, Cards.Spades_6, Cards.Spades_10, Cards.Spades_Ace]);

            Table_cards actual = deck.Deal();

            Assert.IsFalse(expected_north.Equals(actual.North));
            Assert.IsFalse(expected_east.Equals(actual.East));
            Assert.IsFalse(expected_south.Equals(actual.South));
            Assert.IsFalse(expected_west.Equals(actual.West));

            deck = new Deck();
            actual = deck.Deal(shuffle: false);
            Assert.IsTrue(expected_north.Equals(actual.North));
            Assert.IsTrue(expected_east.Equals(actual.East));
            Assert.IsTrue(expected_south.Equals(actual.South));
            Assert.IsTrue(expected_west.Equals(actual.West));
        }

        [Test]
        public void TestDealWithConstraints()
        {
            Deck deck = new Deck();

            List<Hand_constraints> hand_constraints = new List<Hand_constraints>(){
                new Hand_constraints("North       : (13,15)  sssss ¦ hh_ ¦ dd_ ¦ cc_ "),
                new Hand_constraints("East        : (7,10)  ss_ ¦ hh__ ¦ dd__ ¦ cc__ "),
                new Hand_constraints("South       : (8,9)  sss_ ¦ hh_ ¦ dd__ ¦ cc__ "),
            };

            Dictionary<Positions, (HCP_hand_range range, Hand_suits_distribution shape)> Expected_Hand_Distributions = new(){
                {Positions.North, (new HCP_hand_range(13, 15), new Hand_suits_distribution("sssss ¦ hh_ ¦ dd_ ¦ cc_"))},
                {Positions.East, (new HCP_hand_range(8,9), new Hand_suits_distribution("ss_ ¦ hh__ ¦ dd__ ¦ cc__"))},
                {Positions.South, (new HCP_hand_range(8,9), new Hand_suits_distribution("sss_ ¦ hh_ ¦ dd__ ¦ cc__"))},
            };

            Table_cards actual = deck.Deal(hand_constraints);
            Assert.IsTrue(actual.North.HCP_INITIAL >= Expected_Hand_Distributions[Positions.North].range.Min && actual.North.HCP_INITIAL <= Expected_Hand_Distributions[Positions.North].range.Max);

        }

        [Test]
        public void TestReset()
        {
            // Create deck, deal, and verify deck is empty
            Deck deck = new Deck();
            deck.Deal();
            Cards[] cards = deck.GetDeck;
            Assert.That(cards.Length == 0);

            // Reset deck, verify deck is full and not shuffled
            deck.Reset(shuffle: false);
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            cards = deck.GetDeck;

            CollectionAssert.AreEqual(expected_cards, cards);

            // empty deck
            deck.Deal();
            cards = deck.GetDeck;
            Assert.That(cards.Length == 0);

            // Reset deck, verify deck is full and shuffled
            deck.Reset(shuffle: true);
            cards = deck.GetDeck;

            CollectionAssert.AreNotEqual(expected_cards, cards);
        }
    }   
}
