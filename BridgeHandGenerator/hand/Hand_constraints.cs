using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    public class Hand_constraints
    {
        private const string shape_pattern = @"^(?<position>(Not_Assigned|North|East|South|West))\s*: (?<points>\([0-9]+,[0-9]+\))(?<shape>.+)$";
        private static IReadOnlyDictionary<Positions, Hand_constraints> no_constraint_hands = new ReadOnlyDictionary<Positions, Hand_constraints>(new Dictionary<Positions, Hand_constraints>{
            {Positions.North, new(new HCP_hand_range(), new Hand_suits_distribution(), Positions.North)},
            {Positions.East, new(new HCP_hand_range(), new Hand_suits_distribution(), Positions.East)},
            {Positions.South, new(new HCP_hand_range(), new Hand_suits_distribution(), Positions.South)},
            {Positions.West, new(new HCP_hand_range(), new Hand_suits_distribution(), Positions.West)},
        });

        public static ReadOnlyDictionary<Positions, Hand_constraints> NO_CONSTRAINT_HANDS => (ReadOnlyDictionary<Positions, Hand_constraints>)no_constraint_hands;

        public HCP_hand_range Points { get; }

        public Hand_suits_distribution Shape { get; }
        
        public Positions Position { get; set;}

        public int Constraint_level 
        {
            get
            {
                int constraint_level = 0;
                Suit_length[] suits = [Shape.Clubs, Shape.Diamonds, Shape.Hearts, Shape.Spades];

                constraint_level = suits.Where(s => s.Min > 0).Count() * 4;
                if(Points.Max < HCP_hand_range.MAX_HCP) constraint_level += 1;
                if(Points.Min > HCP_hand_range.MIN_HCP) constraint_level += 2;
                return constraint_level;
            }
        }

        public Hand_constraints(HCP_hand_range points, Hand_suits_distribution shape, Positions position)
        {
            if (points == null || shape == null) throw new ApplicationException("'points' or 'shape' cannot be null.");
            
            Points = points;
            Shape = shape;
            Position = position;
        }

        public Hand_constraints(string player_constraints)
        {
            Match match = Regex.Match(player_constraints, shape_pattern);
            if (!match.Success) throw new ApplicationException("Described distribution is invalid.");

            Positions position;

            if (!Enum.TryParse<Positions>(match.Groups["position"].ToString(), out position)) throw new ApplicationException("Described 'position' is not valid.");
            
            Position = position;


            int[] minmax = match.Groups["points"].ToString().Trim([' ', '(', ')']).Split(',', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries).Select(v => int.Parse(v)).ToArray();
            Points = new HCP_hand_range(minmax[0], minmax[1]); // throws an exception if minmax values are outside the valid range for HCP_hand_range

            Shape = new Hand_suits_distribution(match.Groups["shape"].ToString()); // throws an exception if string isn't a valid suits distribution
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (Object.ReferenceEquals(this, obj)) return true;
            
            #pragma warning disable CS8600
            Hand_constraints other = obj as Hand_constraints;
            #pragma warning restore CS8600

            if (other is null) return false;

            return this.Points == other.Points 
                && this.Shape == other.Shape 
                && this.Position == other.Position;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Position,-12}: {this.Points} {this.Shape}";
        }
    }
}