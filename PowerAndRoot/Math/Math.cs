using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerAndRoot
{
    public static class Math
    {
        public static bool Odd(long n)
        {
            return (n & 1L) == 1L;
        }

        public static bool Even(long n)
        {
            return (n & 1L) == 0L;
        }

        public static long Power(long b, long e)
        {
            if (e < 0) return 0L;

            Int64 result = 1;
            Int64 product = b;

            while (e != 0)
            {
                if (Odd(e)) result *= product;
                product *= product;
                e >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static long Root(long n, long radical)
        {
            if (radical < 1)
            {
                throw new ArgumentOutOfRangeException("radical", "Must be a positive number greater than zero.");
            }

            if (n < 0 && Even(radical))
            {
                Exception ex = null;
                throw new ArgumentOutOfRangeException("Even roots cannot be calculated for negative numbers.", ex);
            }

            if (n == 0L || n == 1L || n == -1L)
            {
                return n;
            }

            bool positive = n >= 0L;

            long log = 0;
            long t = positive ? n : -n;
            while (t != 0)
            {
                log++;
                t >>= 1;
            }

            if (radical > log)
            {
                return positive ? 1L : -1L;
            }

            log /= radical;

            long upper = 1L << (int)(log + 1);
            long lower = 1L << (int)(log - 1);
            long root = (upper + lower) >> 1;
            long p, p1;

            while (true)
            {
                p = Power(root, radical);
                if (p > n)
                {
                    upper = root;
                    root = (upper + lower + 1) >> 1;
                    if (root == upper) root--;
                    continue;
                }

                p1 = Power(root + 1, radical);
                if (p1 <= n)
                {
                    lower = root;
                    root = (upper + lower) >> 1;
                    continue;
                }

                break;
            }

            return positive ? root : -root;
        }
    }
}
