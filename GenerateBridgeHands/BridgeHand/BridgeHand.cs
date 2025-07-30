using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.BridgeHand
{
    using Cards;
    using Decks;

    public class BridgeHand
    {
        public const int RemainingHCP = -1;
        public static readonly Dictionary<Value, int> CardValue = new Dictionary<Value, int>
        {
            {Value.A, 4 },
            {Value.K, 3 },
            {Value.Q, 2 },
            {Value.J, 1 },
            {Value._10, 0 },
            {Value._9, 0 },
            {Value._8, 0 },
            {Value._7, 0 },
            {Value._6, 0 },
            {Value._5, 0 },
            {Value._4, 0 },
            {Value._3, 0 },
            {Value._2, 0 }
        };

        private Dictionary<Position, List<Card>> hands = new Dictionary<Position, List<Card>>(4);

        public IReadOnlyCollection<Card> North
        {
            get { return hands[Position.North].OrderByDescending(c => c.Suit).ThenByDescending(c => c.Value).ToList(); }
        }

        public IReadOnlyCollection<Card> East
        {
            get { return hands[Position.East].OrderByDescending(c => c.Suit).ThenByDescending(c => c.Value).ToList(); }
        }

        public IReadOnlyCollection<Card> South
        {
            get { return hands[Position.South].OrderByDescending(c => c.Suit).ThenByDescending(c => c.Value).ToList(); }
        }

        public IReadOnlyCollection<Card> West
        {
            get { return hands[Position.West].OrderByDescending(c => c.Suit).ThenByDescending(c => c.Value).ToList(); }
        }

        /// <summary>
        /// Generates a random hand
        /// </summary>
        public BridgeHand()
        {
            InitHands();

            Deck deck = new Deck();
            deck.Shuffle();
            Position position = Position.North;
            Card card;
            while ((card = deck.Draw()) != null)
            {
                hands[position].Add(card);
                position = (Position)(((int)position + 1) % 4);
            }
        }

        /// <summary>
        /// Generates hands with a minimum of desired HCP per position.
        /// </summary>
        /// <remarks>
        /// A negative value means distribute remaining HCP evenly between positions
        /// </remarks>
        /// <param name="north">Desired minimum HCP for North</param>
        /// <param name="east">Desired minimum HCP for East</param>
        /// <param name="south">Desired minimum HCP for South</param>
        /// <param name="west">Desired minimum HCP for West</param>
        public BridgeHand(
            int north = RemainingHCP,
            int east = RemainingHCP,
            int south = RemainingHCP,
            int west = RemainingHCP)
        {
            int hcp2distribute =
                (north > 0 ? north : 0) +
                (east > 0 ? east : 0) +
                (south > 0 ? south : 0) +
                (west > 0 ? west : 0);

            if (hcp2distribute > 40)
            {
                throw new ArgumentOutOfRangeException("The sum of north, east, south and west cannot exceed 40");
            }

            InitHands();
            Dictionary<Position, int> pointsposition = new Dictionary<Position, int>() {
                { Position.North , north },
                { Position.East , east },
                { Position.South , south },
                { Position.West , west }
            };

            int remaininghcp = 40;

            List<Card> honors = Honors();
            List<Card> nonhonors = NonHonors10();

            Deck.Shuffle(ref honors);
            Deck.Shuffle(ref nonhonors);

            // Distribute desired HCP first, in descending value order
            foreach (KeyValuePair<Position, int> item in pointsposition.Where(kvp => kvp.Value >= 0).OrderByDescending(kvp => kvp.Value))
            {
                int currenthcp = 0;
                while (item.Value > 0 && currenthcp < item.Value && honors.Count > 0)
                {
                    hands[item.Key].Add(honors[0]);
                    currenthcp += CardValue[honors[0].Value];
                    honors.RemoveAt(0);
                }

                remaininghcp -= currenthcp;

                int nonhonorscount = 13 - hands[item.Key].Count;
                for (int i = 0; i < nonhonorscount; i++)
                {
                    hands[item.Key].Add(nonhonors[0]);
                    nonhonors.RemoveAt(0);
                }
            }

            // distribute remaining HCP evenly


            Position[] evendistributedhands = pointsposition.Where(kvp => kvp.Value < 0).Select(kvp => kvp.Key).ToArray();
            int handsforevendistribution = evendistributedhands.Length;
            int initialhcpcount = honors.Count;
            for (int i = 0; i < initialhcpcount; i++)
            {
                hands[evendistributedhands[i % evendistributedhands.Length]].Add(honors[0]);
                honors.RemoveAt(0);
            }

            foreach (KeyValuePair<Position, int> item in pointsposition.Where(kvp => kvp.Value < 0))
            {
                int nonhonorscount = 13 - hands[item.Key].Count;
                for (int i = 0; i < nonhonorscount; i++)
                {
                    hands[item.Key].Add(nonhonors[0]);
                    nonhonors.RemoveAt(0);
                }
            }


        }

        public override string ToString()
        {
            return  string.Join(" ", North.Select(c => c.ToString()))  + 
                " | " + string.Join(" ", East.Select(c => c.ToString())) +
                " | " + string.Join(" ", South.Select(c => c.ToString())) +
                " | " + string.Join(" ", West.Select(c => c.ToString()));
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

            BridgeHand other = obj as BridgeHand;

            if (other == null)
            {
                return false;
            }

            bool isequal = true;
            foreach (Position pos in Enum.GetValues(typeof(Position)))
            {
                for (int i = 0; i < 13 && isequal; i++)
                {
                    isequal &= this.hands[pos][i] == other.hands[pos][i];
                }
            }

            return isequal;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public string PrintHands()
        {
            StringBuilder lines = new StringBuilder();
            lines.Append(new string(' ', 32));
            lines.AppendLine(string.Join(" ", North.Select(c => c.ToString())));
            lines.Append(string.Join(" ", West.Select(c => c.ToString())));
            lines.Append(new string(' ', 20));
            lines.AppendLine(string.Join(" ", East.Select(c => c.ToString())));
            lines.Append(new string(' ', 32));
            lines.AppendLine(string.Join(" ", South.Select(c => c.ToString())));

            return lines.ToString();
        }

        /// <summary>
        /// Generates the A, K, Q, J sets for each suit
        /// </summary>
        /// <returns>
        /// The list of all honors for all suits
        /// </returns>
        private List<Card> Honors()
        {
            List<Card> honors = new List<Card>(16);
            foreach (Suit suit in new[] { Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade })
            {
                foreach (Value value in new[] { Value.A, Value.K, Value.Q, Value.J })
                {
                    honors.Add(new Card(suit, value));
                }
            }

            return honors;
        }

        /// <summary>
        /// Generates the A, K, Q, J, 10 sets for each suit
        /// </summary>
        /// <returns>
        /// The list of all honors including 10 for all suits
        /// </returns>
        private List<Card> Honors10()
        {
            List<Card> honors = new List<Card>(16);
            foreach (Suit suit in new[] { Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade })
            {
                foreach (Value value in new[] { Value.A, Value.K, Value.Q, Value.J, Value._10 })
                {
                    honors.Add(new Card(suit, value));
                }
            }

            return honors;
        }

        /// <summary>
        /// Generates the sets of non-honors (from 2 to 10) for each suit
        /// </summary>
        /// <returns>
        /// The list of all non-honors (from 2 to 10) for all suits
        /// </returns>
        private List<Card> NonHonors10()
        {
            List<Card> honors = new List<Card>(16);
            foreach (Suit suit in new[] { Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade })
            {
                foreach (Value value in new[] { Value._2, Value._3, Value._4, Value._5, Value._6, Value._7, Value._8, Value._9, Value._10 })
                {
                    honors.Add(new Card(suit, value));
                }
            }

            return honors;
        }

        /// <summary>
        /// Generates the sets of non-honors (from 2 to 9) for each suit
        /// </summary>
        /// <returns>
        /// The list of all non-honors (from 2 to 9) for all suits
        /// </returns>
        private List<Card> NonHonors9()
        {
            List<Card> honors = new List<Card>(16);
            foreach (Suit suit in new[] { Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade })
            {
                foreach (Value value in new[] { Value._2, Value._3, Value._4, Value._5, Value._6, Value._7, Value._8, Value._9 })
                {
                    honors.Add(new Card(suit, value));
                }
            }

            return honors;
        }

        private void InitHands()
        {
            foreach (Position pos in Enum.GetValues(typeof(Position)))
            {
                hands.Add(pos, new List<Card>(13));
            }
        }
    }
}
