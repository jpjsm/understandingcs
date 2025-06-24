using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntHash
{
    public static class Utils
    {
        public static int Flip1(this int n)
        {
            int flip = n;
            return ((flip & unchecked((int)0b_1010_1010_1010_1010_1010_1010_1010_1010)) >> 1)
                + ((flip & 0b_0101_0101_0101_0101_0101_0101_0101_0101) << 1);
        }

        public static int Flip2(this int n)
        {
            int flip = n;
            return ((flip & unchecked((int)0b_1100_1100_1100_1100_1100_1100_1100_1100)) >> 2)
                + ((flip & 0b_0011_0011_0011_0011_0011_0011_0011_0011) << 2);
        }

        public static int Flip4(this int n)
        {
            int flip = n;
            return ((flip & unchecked((int)0b_1111_0000_1111_0000_1111_0000_1111_0000)) >> 4)
                + ((flip & 0b_0000_1111_0000_1111_0000_1111_0000_1111) << 4);
        }

        public static int Flip8(this int n)
        {
            int flip = n;
            return ((flip & unchecked((int)0b_1111_1111_0000_0000_1111_1111_0000_0000)) >> 8)
                + ((flip & 0b_0000_0000_1111_1111_0000_0000_1111_1111) << 8);
        }

        public static int Flip16(this int n)
        {
            int flip = n;
            return ((flip & unchecked((int)0b_1111_1111_1111_1111_0000_0000_0000_0000)) >> 16)
                + ((flip & 0b_0000_0000_0000_0000_1111_1111_1111_1111) << 16);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            (b, a) = (a, b);
        }

        public static void SwapElements(ref int[] numbers, int distance = 1)
        {
            if (numbers == null || numbers.Length < 2)
            {
                throw new ArgumentException("'numbers' must contain at least two (2) elements.");
            }

            if (distance < 1)
            {
                throw new ArgumentException("'distance' must be greater than zero (0).");
            }

            if (distance >= numbers.Length)
            {
                throw new ArgumentException("'distance' must be less than 'numbers' length.");
            }

            int size = numbers.Length;

            for (int i = 0; i < size - distance; i+=(2*distance))
            {
                for (int j = 0; j < distance; j++)
                {
                    int left = i + j;
                    int right = i + j + distance;

                    if (right >= size)
                    {
                        break;
                    }

                    Swap(ref numbers[left], ref numbers[right]);
                }
            }
        }
    }
}
