using System;
using System.Collections.Generic;
using System.Text;

namespace vCard
{
    public static class Misc
    {
        public static readonly int[] SmallPrimes = new [] { 127, 179, 233, 283, 353, 419, 467, 547, 607, 661, 739, 811, 877, 947, 131, 181, 239, 293, 359, 421, 479, 557, 613, 673, 743, 821, 881, 953, 137, 191, 241, 307, 367, 431, 487, 563, 617, 677, 751, 823, 883, 967, 139, 193, 251, 311, 373, 433, 491, 569, 619, 683, 757, 827, 887, 971, 149, 197, 257, 313, 379, 439, 499, 571, 631, 691, 761, 829, 907, 977, 101, 151, 199, 263, 317, 383, 443, 503, 577, 641, 701, 769, 839, 911, 983, 103, 157, 211, 269, 331, 389, 449, 509, 587, 643, 709, 773, 853, 919, 991, 107, 163, 223, 271, 337, 397, 457, 521, 593, 647, 719, 787, 857, 929, 997, 109, 167, 227, 277, 347, 401, 461, 523, 599, 653, 727, 797, 859, 937, 113, 173, 229, 281, 349, 409, 463, 541, 601, 659, 733, 809, 863, 941 };

        public static readonly int SmallPrimesLen = SmallPrimes.Length;

        public static Int64 GetHash64(byte[] bytes)
        {
            Int64 hashcode = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hashcode += (bytes[i] * (Misc.SmallPrimes[i % Misc.SmallPrimesLen])) << (i % 2 == 0 ? 0 : 32);
            }

            return hashcode;
        }

        public static Int64 GetHash64(char[] chars)
        {
            Int64 hashcode = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                hashcode += chars[i] * (Misc.SmallPrimes[i % Misc.SmallPrimesLen]);
            }

            return hashcode;
        }

        public static Int64 GetHash64(string text)
        {
            Int64 hashcode = 0;
            string normalized = text.Normalize(NormalizationForm.FormKC);
            byte[] bytes = System.Text.Encoding.UTF32.GetBytes(normalized);
            for (int i = 0; i < bytes.Length; i++)
            {
                hashcode += (bytes[i] * (Misc.SmallPrimes[i % Misc.SmallPrimesLen])) << (i % 2 == 0 ? 0 : 32);
            }

            return hashcode;
        }

    }
}
