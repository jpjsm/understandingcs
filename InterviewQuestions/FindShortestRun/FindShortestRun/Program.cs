using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindShortestRun
{
    class Program
    {
        static void Main(string[] args)
        {
            Tuple<int, int[], int[]>[] unitTests = new Tuple<int, int[], int[]>[]
            {
                new Tuple<int, int[], int[]>(2, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3 }),
                new Tuple<int, int[], int[]>(3, new int[] { 1, 2, 2, 2, 3, 4, 4, 4, 5, 6, 6, 6, 7, 8, 9, 5, 10, 11, 3, 12, 13, 1, 5, 3 }, new int[] { 1, 3, 5 })
            };

            int expected, result;
            for (int i = 0; i < unitTests.Length; i++)
            {
                expected = unitTests[i].Item1;
                result = FindShortestRun(unitTests[i].Item2, unitTests[i].Item3);
                Debug.Assert(
                    expected == result,
                    string.Format("Test run {0} failed! Expected value {1} != {2} result.", i, expected, result));
            }
        }

        public static int FindShortestRun(int[] numbers, int[] set)
        {
            // validate arguments
            if (numbers == null)
            {
                throw new ArgumentNullException(nameof(numbers));
            }

            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            if (set.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(set), "At least two elements are needed to make a run.");
            }

            // TBD: what to do if set contains duplicate values?

            if (numbers.Length < set.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(numbers), "'numbers' set contains less than selection set.");
            }

            int[] lastSeen = new int[set.Length];
            int[] bestSeen = new int[set.Length];
            int bestRun = -1;

            for (int i = 0; i < set.Length; i++)
            {
                lastSeen[i] = bestSeen[i] = -1;
            }

            bool found;
            int min, max;
            for (int i = 0; i < numbers.Length; i++)
            {
                found = false;
                for (int j = 0; j < set.Length; j++)
                {
                    if (numbers[i] == set[j])
                    {
                        lastSeen[j] = i;
                        found = true;
                    }
                }

                if (found)
                {
                    min = Min(lastSeen);
                    if (min != -1)
                    {
                        max = Max(lastSeen);
                        if (bestRun == -1 || bestRun > (max - min + 1))
                        {
                            bestRun = max - min + 1;
                            for (int j = 0; j < set.Length; j++)
                            {
                                bestSeen[j] = lastSeen[j];
                            }
                        }
                    }
                }
            }

            return bestRun;
        }

        public static int Min(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Null or empty array not allowed.");
            }

            int min = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (min > numbers[i])
                {
                    min = numbers[i];
                }
            }

            return min;
        }

        public static int Max(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Null or empty array not allowed.");
            }

            int max = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (max < numbers[i])
                {
                    max = numbers[i];
                }
            }

            return max;
        }
    }
}
