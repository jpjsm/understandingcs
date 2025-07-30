using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Newtonsoft.Json;

namespace NumberTheory
{
    public static class NumberTheory
    {
        public static readonly Int64[]  Primes;
        static NumberTheory()
        {
            Primes = JsonConvert.DeserializeObject<Int64[]>(File.ReadAllText(@"primes.json"));
        }
        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), $"The value of n: {n}, must be greater or equal to zero.");

            if (n == 0)
                return BigInteger.Zero;
             
            if (n < new BigInteger(4))
                return BigInteger.One;

            BigInteger lower = BigInteger.One;
            BigInteger upper = new BigInteger(2);

            while ((upper * upper) <= n)
            {
                lower = upper;
                upper <<= 1;
            }

            BigInteger l2 = lower * lower;

            while (!((l2 <= n) && (n < (l2 + (lower << 1) + 1))))
            {
                BigInteger mid = lower + ((upper - lower + 1) >> 1);
                BigInteger mid2 = mid * mid;
                if (n >= mid2)
                {
                    lower = mid;
                }
                else
                {
                    upper = mid;
                }

                l2 = lower * lower;
            }

            return lower;
        }
    
        public static List<BigInteger> PrimeFactors(this BigInteger n)
        {
            List<BigInteger> factors = new List<BigInteger>() { 1 };
            int i = 0;
            while (Primes[i] <= n)
            {
                if (BigInteger.Remainder(n, Primes[i]) == 0)
                {
                    factors.Add(Primes[i]);
                    n /= Primes[i];
                }
                else
                {
                    i++;
                }

            }

            return factors;
        }

        public static List<long> PrimeFactors(this long n)
        {
            List<long> factors = new List<long>() { 1 };
            int i = 0;
            while (Primes[i] <= n)
            {
                if (BigInteger.Remainder(n, i) == 0)
                {
                    factors.Add(Primes[i]);
                    n /= Primes[i];
                }
                else
                {
                    i++;
                }

            }

            return factors;
        }
    }
}
