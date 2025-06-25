using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    public class Suit_length
    {
        public const int MIN_SUIT_LENGTH = 0;
        public const int MAX_SUIT_LENGTH = 13;
        public int Max { get; }
        public int Min { get; }

        public Suit_length(int? min=null, int? max=null)
        {
            int _min = min ?? MIN_SUIT_LENGTH;
            int _max = max ?? MAX_SUIT_LENGTH;

            if (_max > MAX_SUIT_LENGTH || _min < MIN_SUIT_LENGTH) throw new ApplicationException($"Suit length range must be between {MIN_SUIT_LENGTH} and {MIN_SUIT_LENGTH}.");
            if (_max < _min) throw new ApplicationException("Range must be in ascending order.");

            Max = _max;
            Min = _min;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (Object.ReferenceEquals(this, obj)) return true;
            
            #pragma warning disable CS8600
            Suit_length other = obj as Suit_length;
            #pragma warning restore CS8600
            
            if (other is null) return false;

            return this.Min == other.Min && this.Max == other.Max;
        }

        public override int GetHashCode()
        {
            return (Max<<4)|Min;
        }

        public override string ToString()
        {
            return $"({Min},{Max})";
        }
    }
}