using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitOperations
{
    public static class BitOperations
    {

        public static ulong[] BitsCombinations(byte combinations, byte outof)
        {
            if (outof == 0)
            {
                return null;
            }

            if (combinations == 0)
            {
                return new[] { 0UL };
            }

            if (combinations > outof)
            {
                throw new ApplicationException("Requested 'combinations' need to be Less than or Equal to 'outof'");
            }

            if (outof > 64)
            {
                throw new ApplicationException("Max number of combinations is 64; current request exceeds that value.");
            }

            // resultSize = Fact(outof) / (Fact(outof - combinations) * (Fact(combinations))
            // expanding to avoid calling an external function that works with double value types
            int resultsSize = 1;
            int upper = (outof - combinations) > combinations ? outof - combinations : combinations;
            int lower = (outof - combinations) > combinations ? combinations : outof - combinations;

            for (int i = outof; i > upper; i--)
            {
                resultsSize *= i;
            }

            for (int i = 1; i <= lower; i++)
            {
                resultsSize /= i;
            }

            ulong[] results = new ulong[resultsSize];

            ulong pattern = 0UL;
            for (int i = 0; i < combinations; i++)
            {
                pattern |= (1UL << i);
            }

            ulong endpattern = pattern << (outof - combinations);
            int index = 0;
            ulong smallest, ripple, ones;

            // Generate combinations
            // adapted from: Hacker’s Delight by Henry S. Warren, jr. (2nd Edition) @ Fig 2.1
            while (pattern < endpattern)
            {
                results[index++] = pattern;
                smallest = pattern & ((~pattern) + 1);
                ripple = pattern + smallest;
                ones = pattern ^ ripple;
                ones = (ones >> 2) / smallest;
                pattern = ripple | ones;
            }

            results[index] = endpattern;
            return results;
        }

    }
}
