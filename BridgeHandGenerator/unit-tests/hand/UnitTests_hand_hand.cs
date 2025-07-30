using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace BridgeHandGenerator
{
    public class UnitTests_Hand
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void TestConstructor()
        {
            int expected_HCP = 34;
            ulong expected_Compact_hand = 0b1111000000000111100000000011110000000001000000000000;
            string expected_ToString = $"{Suits.Spades.Print()} A K Q J | {Suits.Hearts.Print()} A K Q J | {Suits.Diamonds.Print()} A K Q J | {Suits.Clubs.Print()} A";

            List<Cards> expected_cards = new List<Cards>([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Hand actual = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Assert.AreEqual(expected_HCP, actual.HCP_INITIAL);
            Assert.AreEqual(expected_HCP, actual.Count_Points());
            Assert.AreEqual(expected_Compact_hand, actual.Compact_hand);
            Assert.AreEqual(expected_cards, actual.Get_Cards);
            Assert.AreEqual(expected_Compact_hand.ToString(), actual.Serialize());
            Assert.AreEqual(expected_ToString, actual.ToString());
            Assert.AreEqual(Hand.Deserialize(actual.Serialize()), actual);
            Assert.AreEqual(Hand.Deserialize(expected_Compact_hand.ToString()), actual);
        }

        [Test]
        public void TestConstructorExceptions()
        {
            List<Cards> empty_hand = [];
            #pragma warning disable CS8600
            List<Cards> null_hand = null;
            #pragma warning restore CS8600
            List<Cards> one_too_many_hand = [
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace, Cards.Clubs_King];
            List<Cards> repeated_values_do_not_count = Enumerable.Repeat(Cards.Clubs_Ace,13).ToList();


            Assert.Throws<ApplicationException>(() => new Hand(empty_hand));
            #pragma warning disable CS8604
            Assert.Throws<ApplicationException>(() => new Hand(null_hand));
            #pragma warning restore CS8604
            Assert.Throws<ApplicationException>(() => new Hand(one_too_many_hand));
            Assert.Throws<ApplicationException>(() => new Hand(repeated_values_do_not_count));

        }

        [Test]
        public void TestCompact_Hand()
        {
            (ulong expected,Cards[] actual)[] values = [
                (0b1111000000000111100000000011110000000001000000000000,[Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,Cards.Clubs_Ace]),
                (((ulong)0b01111111111111<<39), [Cards.Spades_4,Cards.Spades_3,Cards.Spades_2,Cards.Spades_7,Cards.Spades_6,Cards.Spades_5,Cards.Spades_10,Cards.Spades_9,Cards.Spades_8,Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack]),
                (((ulong)0b01111111111111<<26), [Cards.Hearts_4,Cards.Hearts_3,Cards.Hearts_2,Cards.Hearts_7,Cards.Hearts_6,Cards.Hearts_5,Cards.Hearts_10,Cards.Hearts_9,Cards.Hearts_8,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack]),
                (((ulong)0b01111111111111<<13), [Cards.Diamonds_4,Cards.Diamonds_3,Cards.Diamonds_2,Cards.Diamonds_7,Cards.Diamonds_6,Cards.Diamonds_5,Cards.Diamonds_10,Cards.Diamonds_9,Cards.Diamonds_8,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack]),
                (((ulong)0b01111111111111), [Cards.Clubs_4,Cards.Clubs_3,Cards.Clubs_2,Cards.Clubs_7,Cards.Clubs_6,Cards.Clubs_5,Cards.Clubs_10,Cards.Clubs_9,Cards.Clubs_8,Cards.Clubs_Ace, Cards.Clubs_King, Cards.Clubs_Queen, Cards.Clubs_Jack]),
            ];

            foreach (var value in values)
            {
                Assert.AreEqual(value.expected, (new Hand(value.actual)).Compact_hand);
            }
        }

        [Test]
        public void TestShow_Hand()
        {
            List<Cards> expected_cards = new List<Cards>([
                Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Hand hand = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            var _anycard = hand.Play(Cards.Spades_Ace);

            List<Cards> actual_cards = hand.Show_Hand;

            Assert.AreEqual(expected_cards, actual_cards);
            Assert.That(hand.HandInPlay, Is.False);
            Assert.That(hand.Get_Cards.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHandInPlay()
        {
            bool expected_HandInPlay_before_end_of_game = true;
            bool expected_HandInPlay_after_end_of_game = false;

            Hand actual = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Assert.AreEqual(expected_HandInPlay_before_end_of_game, actual.HandInPlay);
            var _anycard = actual.Play(Cards.Spades_Ace);
            Assert.AreEqual(expected_HandInPlay_before_end_of_game, actual.HandInPlay);
            var _anyhand = actual.Show_Hand;
            Assert.AreEqual(expected_HandInPlay_after_end_of_game, actual.HandInPlay);
        }

        [Test]
        public void TestGet_Cards()
        {
            List<Cards> expected_cards = new List<Cards>([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Hand hand = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            while (expected_cards.Count > 0)
            {
                Cards expected_card = expected_cards[0];
                expected_cards.RemoveAt(0);

                Cards actual_card = hand.Play(expected_card);
                Assert.AreEqual(expected_card, actual_card);
            }
        }

        [Test]
        public void TestPlay()
        {
            Cards expected_card = Cards.Diamonds_King;

            Hand hand = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Cards actual_card = hand.Play(expected_card);

            Assert.AreEqual(expected_card, actual_card);

            Assert.Throws<ApplicationException>(() => hand.Play(actual_card));
        }

        [Test]
        public void TestCount_Points()
        {
            int expected_HCP = 34;

            Hand hand = new Hand([
                Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,
                Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,
                Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,
                Cards.Clubs_Ace]);

            Assert.AreEqual(expected_HCP, hand.Count_Points());


            while (hand.Cards_In_Hand > 0)
            {
                Cards card = hand.Get_Cards[0];
                card = hand.Play(card);

                expected_HCP -= card.Card_HCP();
                Assert.AreEqual(expected_HCP, hand.Count_Points());                
            }
        }

        [Test]
        public void TestSerialize()
        {
            (ulong expected,Cards[] actual)[] values = [
                (0b1111000000000111100000000011110000000001000000000000,[Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,Cards.Clubs_Ace]),
                (((ulong)0b01111111111111<<39), [Cards.Spades_4,Cards.Spades_3,Cards.Spades_2,Cards.Spades_7,Cards.Spades_6,Cards.Spades_5,Cards.Spades_10,Cards.Spades_9,Cards.Spades_8,Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack]),
                (((ulong)0b01111111111111<<26), [Cards.Hearts_4,Cards.Hearts_3,Cards.Hearts_2,Cards.Hearts_7,Cards.Hearts_6,Cards.Hearts_5,Cards.Hearts_10,Cards.Hearts_9,Cards.Hearts_8,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack]),
                (((ulong)0b01111111111111<<13), [Cards.Diamonds_4,Cards.Diamonds_3,Cards.Diamonds_2,Cards.Diamonds_7,Cards.Diamonds_6,Cards.Diamonds_5,Cards.Diamonds_10,Cards.Diamonds_9,Cards.Diamonds_8,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack]),
                (((ulong)0b01111111111111), [Cards.Clubs_4,Cards.Clubs_3,Cards.Clubs_2,Cards.Clubs_7,Cards.Clubs_6,Cards.Clubs_5,Cards.Clubs_10,Cards.Clubs_9,Cards.Clubs_8,Cards.Clubs_Ace, Cards.Clubs_King, Cards.Clubs_Queen, Cards.Clubs_Jack]),
            ];

            foreach (var (expected, actual) in values)
            {
                Assert.AreEqual(expected.ToString(), (new Hand(actual)).Serialize());
            }
        }

        [Test]
        public void TestDeserialize()
        {
            (ulong actual,Cards[] expected)[] values = [
                (0b1111000000000111100000000011110000000001000000000000,[Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack,Cards.Clubs_Ace]),
                (((ulong)0b01111111111111<<39), [Cards.Spades_4,Cards.Spades_3,Cards.Spades_2,Cards.Spades_7,Cards.Spades_6,Cards.Spades_5,Cards.Spades_10,Cards.Spades_9,Cards.Spades_8,Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack]),
                (((ulong)0b01111111111111<<26), [Cards.Hearts_4,Cards.Hearts_3,Cards.Hearts_2,Cards.Hearts_7,Cards.Hearts_6,Cards.Hearts_5,Cards.Hearts_10,Cards.Hearts_9,Cards.Hearts_8,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack]),
                (((ulong)0b01111111111111<<13), [Cards.Diamonds_4,Cards.Diamonds_3,Cards.Diamonds_2,Cards.Diamonds_7,Cards.Diamonds_6,Cards.Diamonds_5,Cards.Diamonds_10,Cards.Diamonds_9,Cards.Diamonds_8,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack]),
                (((ulong)0b01111111111111), [Cards.Clubs_4,Cards.Clubs_3,Cards.Clubs_2,Cards.Clubs_7,Cards.Clubs_6,Cards.Clubs_5,Cards.Clubs_10,Cards.Clubs_9,Cards.Clubs_8,Cards.Clubs_Ace, Cards.Clubs_King, Cards.Clubs_Queen, Cards.Clubs_Jack]),
            ];

            foreach (var (actual, expected) in values)
            {
                Assert.AreEqual(new Hand(expected), Hand.Deserialize(actual.ToString()));
            }
        }

        [Test]
        public void TestEquals()
        {
            Hand expected = Hand.Deserialize(0b1111000000000111100000000011110000000001000000000000.ToString());
            Cards[][] hands = [
                [Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack, Cards.Clubs_Ace],
                [Cards.Clubs_Ace, Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack,Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack],
                [Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack, Cards.Clubs_Ace, Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack,Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack],
                [Cards.Hearts_Ace, Cards.Hearts_King, Cards.Hearts_Queen, Cards.Hearts_Jack, Cards.Diamonds_Ace, Cards.Diamonds_King, Cards.Diamonds_Queen, Cards.Diamonds_Jack, Cards.Clubs_Ace, Cards.Spades_Ace, Cards.Spades_King, Cards.Spades_Queen, Cards.Spades_Jack],
            ];

            foreach (Cards[] cards in hands)
            {
                Assert.IsTrue(expected.Equals(new Hand(cards)));
            }

            Assert.IsFalse(expected.Equals(null));
            
            Assert.IsFalse(expected.Equals(hands[0]));

            Hand all_clubs = Hand.Deserialize(0b1111111111111.ToString());
            Assert.IsFalse(expected.Equals(all_clubs));
        }

        [Test]
        public void TestGetHashCode()
        {
            uint[] upper = [0b00001111000001100000000001,0b11111100000000000000000000,0b10101010101010101010101010];
            uint[] lower = [0b10100000001001000000100100,0b00000000000001010101010101,0b00000000000000000000000000];

            for (int i = 0; i < upper.Length; i++)
            {
                int expected = (int)(upper[i] ^ lower[i]);

                ulong compact_hand = ((ulong)upper[i])<<26;
                compact_hand |= (ulong)lower[i];

                Hand hand = Hand.Deserialize(compact_hand.ToString());
                Assert.AreEqual(expected, hand.GetHashCode());
            }
        }

        [Test]
        public void TestToString()
        {
            ulong[] compact_hands = [
                0b0000111100000110000000000110100000001001000000100100,
                0b1111110000000000000000000000000000000001010101010101,
                0b1010101010101010101010101000000000000000000000000000
            ];

            string[] expected_hands = [
                "♠ 10 9 8 7 | ♡ A K 2 | ♢ A Q 4 | ♣ A 7 4",
                "♠ A K Q J 10 9 | ♡ - | ♢ - | ♣ A Q 10 8 6 4 2",
                "♠ A Q 10 8 6 4 2 | ♡ K J 9 7 5 3 | ♢ - | ♣ -"
            ];
            
            for (int i = 0; i < compact_hands.Length; i++)
            {
                Hand hand = Hand.Deserialize(compact_hands[i].ToString());
                string actual = hand.ToString();
                Assert.AreEqual(expected_hands[i], actual);
            }
            foreach (ulong compact_hand in compact_hands)
            {
                Hand actual = Hand.Deserialize(compact_hand.ToString());

            }
        }
    }
}