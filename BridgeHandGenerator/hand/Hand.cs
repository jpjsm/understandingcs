using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    [Serializable]
    public class Hand
    {
        private int hcp_initial;        

        private List<Cards> cards;
        public const int HAND_INITIAL_LENGTH = 13;
        public int HCP_INITIAL => hcp_initial;

        public int Cards_In_Hand => cards.Count;

        public ulong Compact_hand => Compact();

        public List<Cards> Show_Hand 
        {
            get 
            {
                List<Cards> show_hand = new(cards);
                hcp_initial = -1;
                cards.Clear();
                return show_hand;
            }
        }

        public List<Cards> Get_Cards
        {
            get
            {
                List<Cards> show_hand = new(cards);
                return show_hand;
            }
        } 

        public bool HandInPlay => cards != null && cards.Count > 0;
        
        public Hand(IList<Cards> cards)
        {
            if (cards == null 
             || cards.Count != HAND_INITIAL_LENGTH 
             || new HashSet<Cards>(cards).Count != HAND_INITIAL_LENGTH
            ) throw new ApplicationException("Number of cards different of Bridge hand length, or 'cards' is null.");

            this.cards = new List<Cards>(cards);
            hcp_initial = Count_Points();
        }

        public Cards Play(Cards card)
        {
            if(cards.Count == 0) throw new ApplicationException($"Can't play '{card}' from an empty hand.");
            if(!cards.Remove(card)) throw new ApplicationException($"Can't play '{card}', not in hand! ");
            return card;
        }

        public int Count_Points()
        {
            int points = 0;
            foreach(Cards card in cards)
            {
                points += card.Card_HCP();//((ushort)card >> 8) & 0b111;
            }

            return points;
        }

        public string Serialize()
        {
            return Compact().ToString();
        }

        public static Hand Deserialize(string compact_str)
        {
            if (!long.TryParse(compact_str, out long compact_hand)
                || compact_hand < 1L
                || compact_hand > ((long)0b1111111111111) << 39
                ) throw new ApplicationException($"Invalid serialized hand; it must be a positive number between {1L} and {((long)0b1111111111111) << 39}");

            long compact_hand_copy = compact_hand;

            // bit counting, see: https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetKernighan 
            int cards_count = 0;
            for (; compact_hand_copy != 0L; cards_count++)
            {
                compact_hand_copy &= (compact_hand_copy-1);
            }

            if(cards_count > HAND_INITIAL_LENGTH) throw new ApplicationException($"Hand has more than the maximum allowed: {cards_count} > {HAND_INITIAL_LENGTH}");

            List<Cards> cards = [];
            Cards[] sorted_deck = [.. Enum.GetValues(typeof(Cards)).Cast<Cards>().OrderDescending()];
            long card_index = 1;
            for (int i = sorted_deck.Length - 1; i >= 0; i--)
            {
                if((compact_hand & card_index) != 0)
                {
                    cards.Add(sorted_deck[i]);
                }

                card_index <<=1;
            }

            return new Hand(cards);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (Object.ReferenceEquals(this, obj)) return true;

            #pragma warning disable CS8600
            Hand other = obj as Hand;
            #pragma warning restore CS8600

            if (other is null) return false;

            return this.Compact_hand == other.Compact_hand;
        }

        public override int GetHashCode()
        {
            ulong compact = Compact();
            return (int)(compact>>26) ^ (int)(compact & 0x3FFFFFF);
        }

        public override string ToString()
        {
            return Card_Utils.PrintCards(cards);
        }

        
        private ulong Compact()
        {
            ulong compact = 0L;
            foreach (Cards card in cards)
            {
                uint value = (uint)0b1111 & (ushort)card;
                uint suit = (uint)((ushort)card >> 11);
                uint suit_region = 0;
                while((suit & (uint)0b1) == 0) 
                {
                    suit_region += 13;
                    suit >>= 1;
                }

                ushort card_bitmap = (ushort)(0b1 << (int)(value-2));
                compact |= (ulong) ((long)card_bitmap << (int)suit_region);
            }

            return compact;
        }
    }
}