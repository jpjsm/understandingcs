using System;
using System.Numerics;

namespace nearest_prime;

public static class PrimeUtils
{
    public static bool IsPrime(long n)
    {
        if (n < 2) return false;

        long[] small = [
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59,
            61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127,
            131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193,
            197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269,
            271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
            353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431,
            433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503,
            509, 521, 523, 541];

        foreach (var p in small)
        {
            if (n == p) return true;
            if (n % p == 0) return false;
        }

        long d = n - 1;
        int s = 0;
        while ((d & 1) == 0)
        {
            d >>= 1;
            s++;
        }

        bool Check(long a)
        {
            long x = (long)BigInteger.ModPow(a, d, n);
            if (x == 1 || x == n - 1) return true;

            for (int i = 1; i < s; i++)
            {
                x = (long)((BigInteger)x * x % n);
                if (x == n - 1) return true;
            }
            return false;
        }

        long[] bases = { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };
        foreach (var a in bases)
        {
            if (a % n == 0) return true;
            if (!Check(a)) return false;
        }
        return true;
    }

    public static long PreviousPrime(long n)
    {
        if (n < 2) return -1;
        if (n == 2) return 2;

        if ((n & 1) == 0) n--;

        for (long x = n; x >= 2; x -= 2)
        {
            if (IsPrime(x)) return x;
        }
        return -1;
    }

    public static long NextPrime(long n)
    {
        if (n < 2) return -1;
        if (n == 2) return 2;

        if ((n & 1) == 0) n++;

        for (long x = n; x <= 2 * n; x += 2)
        {
            if (IsPrime(x)) return x;
        }
        return -1;
    }
}

class Program
{
    static void Main(string[] args)
    {
        long[] numbers = [1000000L, 5000000, 10000000L, 20000000L, 50000000L, 100000000L, 250000000L, 500000000L, 750000000L, 1000000000L];
        foreach (long number in numbers)
        {
            // Console.WriteLine($"{PrimeUtils.PreviousPrime(number),12:N0} | {PrimeUtils.NextPrime(number),14:N0}");
            Console.WriteLine($"{PrimeUtils.NextPrime(number)},");
        }
    }
}
