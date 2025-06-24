using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Decks
{
    using Game.Cards;

    public class Deck
    {
        private bool inplay = false;
        private bool isShuffled = false;
        private static Random rnd = new Random((int)DateTime.Now.Ticks);
        private List<Card> Pack;

        public Deck()
        {
            Pack = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    Pack.Add(new Card(suit, value));
                }
            }
        }

        public Deck(IEnumerable<Card> cards)
        {
            Pack = cards.ToList();
        }

        public static void Shuffle(ref List<Card> cards)
        {
            List<Card> shuffled = new List<Card>(cards.Count);
            while (cards.Count > 0)
            {
                int index = rnd.Next(cards.Count);
                shuffled.Add(cards[index]);
                cards.RemoveAt(index);
            }

            cards = shuffled;
        }

        public void Shuffle()
        {
            if (!isShuffled)
            {
                Shuffle(ref Pack);
            }
        }

        public List<List<Card>> Deal(int handcount, int cardcount)
        {
            if (handcount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(handcount));
            }

            if (cardcount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cardcount));
            }

            inplay = true;
            if (!isShuffled)
            {
                Shuffle(ref Pack);
            }

            List<List<Card>> hands = new List<List<Card>>();
            for (int i = 0; i < handcount; i++)
            {
                hands.Add(new List<Card>());
            }

            for (int i = 0; i < cardcount; i++)
            {
                for (int j = 0; j < handcount && Pack.Count > 0; j++)
                {
                    hands[i].Add(Pack[0]);
                    Pack.RemoveAt(0);
                }
            }

            return hands;
        }

        public Card Draw()
        {
            Card card = null;

            inplay = true;
            if (Pack.Count > 0)
            {
                card = Pack[0];
                Pack.RemoveAt(0);
            }

            return card;
        }

        public void PushbackIn(Card card)
        {
            Pack.Insert(0, card);
        }
    }
}
