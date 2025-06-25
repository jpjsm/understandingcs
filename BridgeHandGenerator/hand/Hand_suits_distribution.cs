using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    public class Hand_suits_distribution
    
    {
        private const string distribution_pattern = @"^\s*(?<spades>(-|_+|s+(-|_+)?))\s+[¦|]\s+(?<hearts>(-|_+|h+(-|_+)?))\s+[¦|]\s+(?<diamonds>(-|_+|d+(-|_+)?))\s+[¦|]\s+(?<clubs>(-|_+|c+(-|_+)?))\s*$";
        private Dictionary<Suits, Suit_length> suits = new(){
            {Suits.Spades, new Suit_length()},
            {Suits.Hearts, new Suit_length()},
            {Suits.Diamonds, new Suit_length()},
            {Suits.Clubs, new Suit_length()},

        };

        public const int TOTAL_CARDS = 13;
        public Suit_length Spades => suits[Suits.Spades];
        public Suit_length Hearts => suits[Suits.Hearts];
        public Suit_length Diamonds => suits[Suits.Diamonds];
        public Suit_length Clubs => suits[Suits.Clubs];
        public IEnumerable<(Suits, Suit_length)> Suit_restrictions => suits.Where(s => s.Value != null).Select(s => (s.Key, s.Value)).ToArray();
        

        public Hand_suits_distribution
        (Suit_length spades, Suit_length hearts, Suit_length diamonds, Suit_length clubs)
        {
            suits[Suits.Spades] = spades;
            suits[Suits.Hearts] = hearts;
            suits[Suits.Diamonds] = diamonds;
            suits[Suits.Clubs] = clubs;

            int mins_total = suits.Sum(s => s.Value.Min);
            if (mins_total > TOTAL_CARDS) throw new ApplicationException("Minimum number of cards exceeds total number of cards.");

            int maxs_total = suits.Sum(s => s.Value.Max);
            if (maxs_total < TOTAL_CARDS) throw new ApplicationException("Maximum number of cards is less than total number of cards.");
        }

        public Hand_suits_distribution()
            : this(new Suit_length(), new Suit_length(), new Suit_length(), new Suit_length())
        {            
        }

        public Hand_suits_distribution(IList<(Suits suit, Suit_length length)> _suits)
            : this()
        {
            if(_suits == null || _suits.Count == 0) return;

            foreach (var (_suit, length) in _suits)
            {
                if (!Suit_Utils.Card_suits.Contains(_suit)) throw new ApplicationException($"'{_suit}' not a deck suit.");

                suits[_suit] = length;
            }
        }

        public Hand_suits_distribution(string distribution)
        {
            Match match = Regex.Match(distribution, distribution_pattern);
            if (!match.Success) throw new ApplicationException($"Invalid distribution pattern: {distribution_pattern}");


            foreach (Suits suit in suits.Keys)
            {
                string suit_name = Enum.GetName(typeof(Suits), suit).ToLowerInvariant();
                string definition = match.Groups[suit_name].ToString().ToLowerInvariant();
                 

                if (definition == "-") 
                {
                    suits[suit] = new Suit_length();
                    continue;
                }

                if (definition.StartsWith('_')) 
                {
                    suits[suit] = new Suit_length(null, definition.Length); // throws an exception if definition.Length is outside the valid range for Suit_length
                    continue;
                }

                Dictionary<char, int> frequencies = definition.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

                if(!frequencies.ContainsKey(suit_name[0])) throw new ApplicationException($"Suit shape of the wrong kind; expecting '{suit_name[0]}' for '{suit}'");

                if (frequencies.Count == 1) 
                {
                    suits[suit] = new Suit_length(frequencies[suit_name[0]], frequencies[suit_name[0]]);// throws an exception if frequencies[suit[0]]) is outside the valid range for Suit_length
                    continue;
                }

                if (definition[^1] == '_') 
                {
                    suits[suit] = new Suit_length(frequencies[suit_name[0]], frequencies[suit_name[0]]+frequencies['_']);// throws an exception if frequencies[suit[0]]) or frequencies['_'] are outside the valid range for Suit_length
                    continue;
                }

                if (definition[^1] == '-') 
                {
                    suits[suit] = new Suit_length(frequencies[suit_name[0]], Suit_length.MAX_SUIT_LENGTH);// throws an exception if frequencies[suit[0]]) is outside the valid range for Suit_length
                    continue;
                }
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (Object.ReferenceEquals(this, obj)) return true;
            
            #pragma warning disable CS8600
            Hand_suits_distribution other = obj as Hand_suits_distribution;
            #pragma warning restore CS8600

            if (other is null) return false;

            return this.Spades.Equals(other.Spades) 
                && this.Hearts.Equals(other.Hearts)
                && this.Diamonds.Equals(other.Diamonds)
                && this.Clubs.Equals(other.Clubs);
        }

        public override int GetHashCode()
        {
            return this.Spades.GetHashCode() << 24 
                 + this.Hearts.GetHashCode() << 16
                 + this.Diamonds.GetHashCode() << 8
                 + this.Clubs.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder results = new StringBuilder();

            (char, Suit_length)[] shapes = [('s', this.Spades), ('h', this.Hearts), ('d', this.Diamonds), ('c', this.Clubs)];
            foreach((char suit, Suit_length length) shape in shapes)
            {
                if (shape.length.Min == Suit_length.MIN_SUIT_LENGTH 
                    && shape.length.Max == Suit_length.MAX_SUIT_LENGTH)
                {
                    results.Append(" - ¦");
                    continue;
                }

                if (shape.length.Min == Suit_length.MIN_SUIT_LENGTH)
                {
                    results.Append($" {new string('_', shape.length.Max)} ¦");
                    continue;
                }

                if (shape.length.Max == Suit_length.MAX_SUIT_LENGTH)
                {
                    results.Append($" {new string(shape.suit, shape.length.Min)}- ¦");
                    continue;
                }

                results.Append($" {new string(shape.suit, shape.length.Min)}{(shape.length.Max == shape.length.Min ? string.Empty : new string('_', shape.length.Max-shape.length.Min))} ¦");
            }

            return results.ToString()[0..^1];
        }
    }
}
