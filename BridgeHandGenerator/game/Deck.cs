using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeHandGenerator;

namespace BridgeHandGenerator
{
    public class Deck
    {
        private const string in_hand = "in_hand";
        private const string pile = "pile";
        private List<Cards> deck = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToList();
        private static readonly Random rnd = new Random();
        private static uint shuffle_passes = 2;

        public static int NUMBER_OF_CARDS => Enum.GetValues(typeof(Cards)).Cast<Cards>().ToList().Count;

        public int CARDS_IN_DECK => deck.Count;
        
        public Deck() 
        { 
        }

        public Deck(uint shuffle_times)
        {
            shuffle_passes = shuffle_times;
            Shuffle();
        }

        public void Reset(bool shuffle=false, uint shuffle_times = 2) 
        {
            deck = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToList();
            shuffle_passes = shuffle_times;

            if (shuffle) Shuffle();
        }

        public void Shuffle() 
        {
            for (uint shuffle_pass = 0; shuffle_pass < shuffle_passes; shuffle_pass++)
            {
                for (int j = 0; j < deck.Count; j++)
                {
                    Cards card = deck[j];
                    int swap_with = rnd.Next(deck.Count);
                    deck[j] = deck[swap_with];
                    deck[swap_with] = card;
                }
            }
        }

        public Cards[] GetDeck { get { return [.. deck]; } }

        public Cards Card => deck.Dequeue();

        public Table_cards Deal(bool shuffle=true)
        {
            if (deck.Count != Enum.GetValues(typeof(Cards)).Cast<Cards>().Count()) throw new ApplicationException("Deck already dealt.");

            if (shuffle) Shuffle();

            List<Cards>[] hands = [[], [], [], []];


            int recipient = 0;
            while (deck.Count > 0) 
            {
                hands[recipient++ % 4].Add(deck.Dequeue());
            }

            return new Table_cards(new Hand(hands[0]), new Hand(hands[1]), new Hand(hands[2]), new Hand(hands[3]));
        }

        public Table_cards Deal(List<Hand_constraints> hand_constraints)
        {
            if (hand_constraints == null || hand_constraints.Count == 0)
            {
                return Deal();
            }

            if (deck.Count != Deck.NUMBER_OF_CARDS) throw new ApplicationException("Deck already dealt.");

            Shuffle();

            Add_no_constraint_missing_positions(hand_constraints);

            Dictionary<Positions, Hand_constraints> position_constraints = hand_constraints.ToDictionary(c => c.Position, c => c);

            Dictionary<Positions, Hand> hands = [];

            Dictionary<string, List<Cards>> cards = new Dictionary<string, List<Cards>>(){
                {pile, new()},
                {in_hand, deck}
            };

            List<Hand_constraints> hands_order_by_constraints = hand_constraints.OrderBy(c => c.Constraint_level).ToList();

            foreach (Hand_constraints hand_constraint in hands_order_by_constraints)
            {
                List<Cards> hand_cards = [];
                
                Dictionary<Suits, int> suit_counts = new() { 
                    { Suits.Clubs, 0 }, 
                    { Suits.Diamonds, 0 }, 
                    { Suits.Hearts, 0 }, 
                    { Suits.Spades, 0 } 
                };

                int hand_points = 0;

                // Get suit constraints and sort them by complexity level, then by suit
                List<(Suits suit, int level)> suit_constraints_levels =
                [
                    (suit: Suits.Clubs, level: (hand_constraint.Shape.Clubs.Min > 0 ? 2 : 0) + (hand_constraint.Shape.Clubs.Max < 13 ? 1 : 0)),
                    (suit: Suits.Diamonds, level: (hand_constraint.Shape.Diamonds.Min > 0 ? 2 : 0) + (hand_constraint.Shape.Diamonds.Max < 13 ? 1 : 0)),
                    (suit: Suits.Hearts, level: (hand_constraint.Shape.Hearts.Min > 0 ? 2 : 0) + (hand_constraint.Shape.Hearts.Max < 13 ? 1 : 0)),
                    (suit: Suits.Spades, level: (hand_constraint.Shape.Spades.Min > 0 ? 2 : 0) + (hand_constraint.Shape.Spades.Max < 13 ? 1 : 0)),
                ];

                suit_constraints_levels = [.. suit_constraints_levels.OrderByDescending(s => s.level).ThenByDescending(s => s.suit)];

                // Fulfill min HCP
                do
                {
                    Cards card = cards[in_hand].Dequeue(); 
                    bool counted = false;

                    if (card.Card_HCP() > 0 && hand_points + card.Card_HCP() < hand_constraint.Points.Max)
                    {
                    switch (card.Card_Suit())
                        {
                            case Suits.Spades:
                                if (suit_counts[Suits.Spades] < hand_constraint.Shape.Spades.Max)
                                {
                                    suit_counts[Suits.Spades]++;
                                    counted = true;
                                }
                                break;
                            case Suits.Hearts:
                                if (suit_counts[Suits.Hearts] < hand_constraint.Shape.Hearts.Max)
                                {
                                    suit_counts[Suits.Hearts]++;
                                    counted = true;
                                }
                                break;
                            case Suits.Diamonds:
                                if (suit_counts[Suits.Diamonds] < hand_constraint.Shape.Diamonds.Max)
                                {
                                    suit_counts[Suits.Diamonds]++;
                                    counted = true;
                                }
                                break;
                            case Suits.Clubs:
                                if (suit_counts[Suits.Clubs] < hand_constraint.Shape.Clubs.Max)
                                {
                                    suit_counts[Suits.Clubs]++;
                                    counted = true;
                                }
                                break;                        
                            default:
                                throw new ApplicationException($"Unexpected suit '{card.Card_Suit()}'");
                        }
                    }

                    if (counted)
                    {
                        hand_cards.Add(card);
                        hand_points += card.Card_HCP();
                    }
                    else
                    {
                        cards[pile].Enqueue(card);                            
                    }
                } while(!cards[in_hand].IsEmpty() && hand_cards.Count < Hand_suits_distribution.TOTAL_CARDS);

                // Fulfill min requirements
                while(!cards[pile].IsEmpty())
                {
                    cards[in_hand].Enqueue(cards[pile].Dequeue());
                }

                do
                {
                    Cards card = cards[in_hand].Dequeue(); 
                    bool counted = false;
                    if (hand_points + card.Card_HCP() > hand_constraint.Points.Min)
                    {
                        cards[pile].Enqueue(card);
                        continue;
                    }

                    switch (card.Card_Suit())
                    {
                        case Suits.Spades:
                            if (hand_constraint.Shape.Spades.Min > 0 && suit_counts[Suits.Spades] < hand_constraint.Shape.Spades.Min)
                            {
                                suit_counts[Suits.Spades]++;
                                counted = true;
                            }
                            break;
                        case Suits.Hearts:
                            if (hand_constraint.Shape.Hearts.Min > 0 && suit_counts[Suits.Hearts] < hand_constraint.Shape.Hearts.Min)
                            {
                                suit_counts[Suits.Hearts]++;
                                counted = true;
                            }
                            break;
                        case Suits.Diamonds:
                            if (hand_constraint.Shape.Diamonds.Min > 0 && suit_counts[Suits.Diamonds] < hand_constraint.Shape.Diamonds.Min)
                            {
                                suit_counts[Suits.Diamonds]++;
                                counted = true;
                            }
                            break;
                        case Suits.Clubs:
                            if (hand_constraint.Shape.Clubs.Min > 0 && suit_counts[Suits.Clubs] < hand_constraint.Shape.Clubs.Min)
                            {
                                suit_counts[Suits.Clubs]++;
                                counted = true;
                            }
                            break;                        
                        default:
                            throw new ApplicationException($"Unexpected suit '{card.Card_Suit()}'");
                    }

                    if (counted)
                    {
                        hand_cards.Add(card);
                        hand_points += card.Card_HCP();
                    }
                    else
                    {
                        cards[pile].Enqueue(card);                            
                    }
                }while (!cards[in_hand].IsEmpty() && hand_cards.Count < Hand_suits_distribution.TOTAL_CARDS &&
                    ((hand_constraint.Shape.Spades.Min > 0 && suit_counts[Suits.Spades] < hand_constraint.Shape.Spades.Min) ||
                     (hand_constraint.Shape.Hearts.Min > 0 && suit_counts[Suits.Hearts] < hand_constraint.Shape.Hearts.Min) ||
                     (hand_constraint.Shape.Diamonds.Min > 0 && suit_counts[Suits.Diamonds] < hand_constraint.Shape.Diamonds.Min) ||
                     (hand_constraint.Shape.Clubs.Min > 0 && suit_counts[Suits.Clubs] < hand_constraint.Shape.Clubs.Min)
                    ));

                // Fulfill max requirements
                while(!cards[pile].IsEmpty())
                {
                    cards[in_hand].Enqueue(cards[pile].Dequeue());
                }

                while (hand_cards.Count < Hand_suits_distribution.TOTAL_CARDS && !cards[in_hand].IsEmpty())
                {
                    Cards card = cards[in_hand].Dequeue(); 
                    bool counted = false;
                    if (hand_points + card.Card_HCP() > hand_constraint.Points.Max)
                    {
                        cards[pile].Enqueue(card);
                        continue;
                    }

                    switch (card.Card_Suit())
                    {
                        case Suits.Spades:
                            if (hand_constraint.Shape.Spades.Max < Hand_suits_distribution.TOTAL_CARDS && suit_counts[Suits.Spades] < hand_constraint.Shape.Spades.Max)
                            {
                                suit_counts[Suits.Spades]++;
                                counted = true;
                            }
                            break;
                        case Suits.Hearts:
                            if (hand_constraint.Shape.Hearts.Max < Hand_suits_distribution.TOTAL_CARDS && suit_counts[Suits.Hearts] < hand_constraint.Shape.Hearts.Max)
                            {
                                suit_counts[Suits.Hearts]++;
                                counted = true;
                            }
                            break;
                        case Suits.Diamonds:
                            if (hand_constraint.Shape.Diamonds.Max < Hand_suits_distribution.TOTAL_CARDS && suit_counts[Suits.Diamonds] < hand_constraint.Shape.Diamonds.Max)
                            {
                                suit_counts[Suits.Diamonds]++;
                                counted = true;
                            }
                            break;
                        case Suits.Clubs:
                            if (hand_constraint.Shape.Clubs.Min < Hand_suits_distribution.TOTAL_CARDS && suit_counts[Suits.Clubs] < hand_constraint.Shape.Clubs.Max)
                            {
                                suit_counts[Suits.Clubs]++;
                                counted = true;
                            }
                            break;                        
                        default:
                            throw new ApplicationException($"Unexpected suit '{card.Card_Suit()}'");
                    }

                    if (counted)
                    {
                        hand_cards.Add(card);
                        hand_points += card.Card_HCP();
                    }
                    else
                    {
                        cards[pile].Enqueue(card);                            
                    }
                }

                // Complete number of cards
                while(!cards[pile].IsEmpty())
                {
                    cards[in_hand].Enqueue(cards[pile].Dequeue());
                }

                while (hand_cards.Count < Hand_suits_distribution.TOTAL_CARDS && !cards[in_hand].IsEmpty())
                {
                    Cards card = cards[in_hand].Dequeue(); 
                    bool counted = false;
                    if (hand_points + card.Card_HCP() > hand_constraint.Points.Max)
                    {
                        cards[pile].Enqueue(card);
                        continue;
                    }

                    switch (card.Card_Suit())
                    {
                        case Suits.Spades:
                            if (hand_constraint.Shape.Spades.Max == Hand_suits_distribution.TOTAL_CARDS)
                            {
                                suit_counts[Suits.Spades]++;
                                counted = true;
                            }
                            break;
                        case Suits.Hearts:
                            if (hand_constraint.Shape.Hearts.Max == Hand_suits_distribution.TOTAL_CARDS)
                            {
                                suit_counts[Suits.Hearts]++;
                                hand_cards.Add(card);
                            }
                            break;
                        case Suits.Diamonds:
                            if (hand_constraint.Shape.Diamonds.Max == Hand_suits_distribution.TOTAL_CARDS)
                            {
                                suit_counts[Suits.Diamonds]++;
                                hand_cards.Add(card);
                            }
                            break;
                        case Suits.Clubs:
                            if (hand_constraint.Shape.Clubs.Min == Hand_suits_distribution.TOTAL_CARDS)
                            {
                                suit_counts[Suits.Clubs]++;
                                hand_cards.Add(card);
                            }
                            break;                        
                        default:
                            throw new ApplicationException($"Unexpected suit '{card.Card_Suit()}'");
                    }

                    if (counted)
                    {
                        hand_cards.Add(card);
                        hand_points += card.Card_HCP();
                    }
                    else
                    {
                        cards[pile].Enqueue(card);                            
                    }
                }
                
                // Verify hand is complete, if not try to switch cards with other hands
                // -> if not possible to switch cards with other built hands throw exception
                if (hand_cards.Count != Hand_suits_distribution.TOTAL_CARDS)
                {
                    if (hands.Count < 1)
                    {
                        throw new ApplicationException($"Impossible to fulfill constraints for 1st hand:\n\thand constraints: {hand_constraint}\n\thand built: {Card_Utils.PrintCards(hand_cards)}\n\tin hand content: {Card_Utils.PrintCards(deck)}\n\tpile content: {Card_Utils.PrintCards(cards[pile])}");
                    }

                    // Identify the problem
                    // - min points not met
                    if (Card_Utils.HCPs(hand_cards) < hand_constraint.Points.Min)
                    {
                        int points_min_diff = hand_constraint.Points.Min - Card_Utils.HCPs(hand_cards);
                        int points_max_diff =  hand_constraint.Points.Max - Card_Utils.HCPs(hand_cards);
                        foreach (var (_position, _hand) in hands)
                        {
                            
                        }
                    }

                    // - min suit distribution not met
                    foreach (var suit_restriction in hand_constraint.Shape.Suit_restrictions)
                    {
                        
                    }

                    // print built hands
                    foreach (var _hand_constraints in hands_order_by_constraints)
                    {
                        var _position = _hand_constraints.Position;
                        if (hands.ContainsKey(_position))
                        {
                            Console.WriteLine($"{_hand_constraints} -> ({hands[_position].HCP_INITIAL}) {hands[_position]}");                            
                        }
                    }

                    throw new ApplicationException($"Impossible to fulfill constraints for hand # {hands.Count + 1}:\n\thand constraints: {hand_constraint}\n\thand built: ({Card_Utils.HCPs(hand_cards)}) {Card_Utils.PrintCards(hand_cards)}\n\tin hand content: {Card_Utils.PrintCards(deck)}\n\tpile content: {Card_Utils.PrintCards(cards[pile])}");
                }

                hands[hand_constraint.Position]=new Hand(hand_cards);
            }

            
            return new Table_cards(
                hands[Positions.North], 
                hands[Positions.East], 
                hands[Positions.South], 
                hands[Positions.West]
            );
        }

        private void Add_no_constraint_missing_positions(List<Hand_constraints> hand_Constraints)
        {
            HashSet<Positions> missing_positions = new HashSet<Positions>( Enum.GetValues(typeof(Positions)).Cast<Positions>().ToList());
            foreach (Hand_constraints hand_constraint in hand_Constraints)
            {
                missing_positions.Remove(hand_constraint.Position);
            }

            foreach (Positions missing_position in missing_positions)
            {
                hand_Constraints.Add(Hand_constraints.NO_CONSTRAINT_HANDS[missing_position]);
            }

        }
    }
}
