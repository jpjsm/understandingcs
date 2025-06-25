using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BridgeHandGenerator
{
#pragma warning disable format
    public enum Cards:ushort
    {
        Spades_Ace     = 0b0100010000001110,
        Spades_King    = 0b0100001100001101,
        Spades_Queen   = 0b0100001000001100,
        Spades_Jack    = 0b0100000100001011,
        Spades_10      = 0b0100000000001010,
        Spades_9       = 0b0100000000001001,
        Spades_8       = 0b0100000000001000,
        Spades_7       = 0b0100000000000111,
        Spades_6       = 0b0100000000000110,
        Spades_5       = 0b0100000000000101,
        Spades_4       = 0b0100000000000100,
        Spades_3       = 0b0100000000000011,
        Spades_2       = 0b0100000000000010,

        Hearts_Ace     = 0b0010010000001110,
        Hearts_King    = 0b0010001100001101,
        Hearts_Queen   = 0b0010001000001100,
        Hearts_Jack    = 0b0010000100001011,
        Hearts_10      = 0b0010000000001010,
        Hearts_9       = 0b0010000000001001,
        Hearts_8       = 0b0010000000001000,
        Hearts_7       = 0b0010000000000111,
        Hearts_6       = 0b0010000000000110,
        Hearts_5       = 0b0010000000000101,
        Hearts_4       = 0b0010000000000100,
        Hearts_3       = 0b0010000000000011,
        Hearts_2       = 0b0010000000000010,

        Diamonds_Ace   = 0b0001010000001110,
        Diamonds_King  = 0b0001001100001101,
        Diamonds_Queen = 0b0001001000001100,
        Diamonds_Jack  = 0b0001000100001011,
        Diamonds_10    = 0b0001000000001010,
        Diamonds_9     = 0b0001000000001001,
        Diamonds_8     = 0b0001000000001000,
        Diamonds_7     = 0b0001000000000111,
        Diamonds_6     = 0b0001000000000110,
        Diamonds_5     = 0b0001000000000101,
        Diamonds_4     = 0b0001000000000100,
        Diamonds_3     = 0b0001000000000011,
        Diamonds_2     = 0b0001000000000010,

        Clubs_Ace      = 0b0000110000001110,
        Clubs_King     = 0b0000101100001101,
        Clubs_Queen    = 0b0000101000001100,
        Clubs_Jack     = 0b0000100100001011,
        Clubs_10       = 0b0000100000001010,
        Clubs_9        = 0b0000100000001001,
        Clubs_8        = 0b0000100000001000,
        Clubs_7        = 0b0000100000000111,
        Clubs_6        = 0b0000100000000110,
        Clubs_5        = 0b0000100000000101,
        Clubs_4        = 0b0000100000000100,
        Clubs_3        = 0b0000100000000011,
        Clubs_2        = 0b0000100000000010,
    }
#pragma warning restore format

    public static class Card_Utils
    {
        public static int Card_HCP(this Cards card)
        {
            return ((ushort)card >> 8) & 0b111;            
        }

        public static Suits Card_Suit(this Cards card)
        {
            return (Suits)(((ushort)card >> 11) & 0b1111);
        }

        public static T Dequeue<T>(this List<T> list)
        {
            if (list.Count == 0) throw new ApplicationException("Cannot remove item from empty list.");
            T item = list[0];
            list.RemoveAt(0);
            return item;
        }

        public static void Enqueue<T>(this List<T> list, T item)
        {
            list.Add(item);
            return ;
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static string Value2Str(this Cards card)
        {
            int value = 0b1111 & (ushort)card;

            switch (value)
            {
                case 14:
                    return "A";

                case 13:
                    return "K";

                case 12:
                    return "Q";

                case 11:
                    return "J";

                default:
                    if(value>10 || value < 2) throw new ApplicationException("Invalid card value.");
                    return value.ToString();
            }
        }

        public static string PrintCards(IEnumerable<Cards> cards)
        {
            StringBuilder results = new();

            foreach (Suits suit in new []{Suits.Spades, Suits.Hearts, Suits.Diamonds, Suits.Clubs})
            {
                string[] suit_cards = cards.Where(c => c.Card_Suit() == suit).OrderByDescending(c => c).Select(c => c.Value2Str()).ToArray();
                results.Append($"{suit.Print()} " + (suit_cards.Length == 0 ? "-" : String.Join(' ', suit_cards)) + " | ");
            };

            return results.ToString()[..^3];
        }

        public static int HCPs(IEnumerable<Cards> cards)
        {
            int hcps = 0;

            foreach (Cards card in cards)
            {
                hcps += card.Card_HCP();
            }

            return hcps;
        }
    }
}