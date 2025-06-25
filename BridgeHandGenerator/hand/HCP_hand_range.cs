using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    public class HCP_hand_range
    {
        public const int MIN_HCP = 0;
        public const int MAX_HCP = 37;
        public int Max { get; }
        public int Min { get; }

        public HCP_hand_range(int? min=null, int? max=null)
        {
            int _min = min ?? MIN_HCP;
            int _max = max ?? MAX_HCP;

            if (_max > MAX_HCP || _min < MIN_HCP) throw new ApplicationException($"Suit length range must be between {MIN_HCP} and {MAX_HCP}.");
            if (_max < _min) throw new ApplicationException("Range must be in ascending order.");

            Max = _max;
            Min = _min;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (Object.ReferenceEquals(this, obj)) return true;
            
            #pragma warning disable CS8600
            HCP_hand_range other = obj as HCP_hand_range;
            #pragma warning restore CS8600

            if (other is null) return false;

            return this.Min == other.Min && this.Max == other.Max;
        }

        public override int GetHashCode()
        {
            return (Max<<6)|Min;
        }

        public override string ToString()
        {
            return $"({Min},{Max})";
        }

    }
}