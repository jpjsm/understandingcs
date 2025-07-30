using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Cards
{
    public class Card
    {
        public Suit Suit { get; private set; }
        public Value Value { get; private set; }

        public Card(Suit suit, Value value)
        {
            if (!Enum.IsDefined(typeof(Suit), suit))
            {
                throw new ArgumentOutOfRangeException(nameof(suit), suit.ToString());
            }

            if (!Enum.IsDefined(typeof(Value), value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value.ToString());
            }

            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(Suit),Suit)[0] + Enum.GetName(typeof(Value), Value).TrimStart('_');
        }

        public override int GetHashCode()
        {
            return ((int)Suit)*15+((int)Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (System.Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            Card other = obj as Card;

            if (other == null)
            {
                return false;
            }

            return other.Suit == this.Suit && other.Value == this.Value ;
        }

        public string Print()
        {
            string face;
            switch (Suit)
            {
                case Suit.Club:
                    face = '\u2663'.ToString() ;
                    break;
                case Suit.Diamond:
                    face = '\u2662'.ToString() ;
                    break;
                case Suit.Heart:
                    face = '\u2661'.ToString() ;
                    break;
                case Suit.Spade:
                    face = '\u2660'.ToString();
                    break;
                default:
                    face = "\u1F0A0" ;
                    break;
            }
            return face + Enum.GetName(typeof(Value), Value).TrimStart('_');
        }
    }
}
