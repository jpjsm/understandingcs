using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topography
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            Point other = obj as Point;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            uint x = (uint)this.X;
            uint y = (uint)this.Y;

            uint x1 = ((x & 0xAAAAAAAA) >> 1) | ((x & 0x55555555) << 1);
            uint y1 = ((y & 0xFFFF0000) >> 16) | ((y & 0x0000FFFF) << 16);
            return (int)(x1 ^ y1);
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }
    }
}
