using System;
using System.Collections.Immutable;
using BridgeHandGenerator;

namespace BridgeHandGenerator
{
#pragma warning disable format
    public enum Suits:byte
    {
        Not_Assigned     = 0b00000,
        Clubs            = 0b00001,
        Diamonds         = 0b00010,
        Hearts           = 0b00100,
        Spades           = 0b01000,
        NoTrump          = 0b10000,
    }
#pragma warning restore format

    public static class Suit_Utils
    {
        public readonly static ImmutableArray<Suits> Deck_Suits = ImmutableArray.Create([Suits.Spades, Suits.Hearts, Suits.Diamonds, Suits.Clubs]);

        public readonly static Suits[] Card_suits = [Suits.Spades, Suits.Hearts, Suits.Diamonds, Suits.Clubs];
        public static string Print(this Suits suit)
        {
            return suit switch
            {
                Suits.Clubs => "\u2663",
                Suits.Diamonds => "\u2662",
                Suits.Hearts => "\u2661",
                Suits.Spades => "\u2660",
                Suits.NoTrump => "NT",
                Suits.Not_Assigned => "\U0001f0a0",
                _ => throw new ApplicationException($"Not a defined suit: {(byte)suit}"),
            };
        }
    }
}

