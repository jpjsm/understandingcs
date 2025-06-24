using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveDuplicates
{
    class Program
    {
        static void Main(string[] args)
        {
            Tuple<int[], int[]>[] unitTests = new Tuple<int[], int[]>[]
                {
                    new Tuple<int[], int[]>(new [] { 9}, new [] { 9}),
                    new Tuple<int[], int[]>(new [] { 1, 2, 3, 4, 5, 6}, new [] { 1, 2, 3, 4, 5, 6}),
                    new Tuple<int[], int[]>(new [] { 1, 2, 3 }, new int[] { 3, 3, 3, 2, 2, 1})
                };

            int results;

            for (int i = 0; i < unitTests.Length; i++)
            {
                results = RemoveDuplicates(unitTests[i].Item2);
                for (int j = 0; j <= results; j++)
                {
                    Debug.Assert(
                        unitTests[i].Item1[j] == unitTests[i].Item2[j],
                        string.Format(
                            "Unit test: {0}, failed at element {1}! values: {2} != {3}",
                            i,
                            j,
                            unitTests[i].Item1[j],
                            unitTests[i].Item2[j]));
                }
            }
        }

        /// <summary>
        /// Rearranges the array to have all distinct values in argument on the left side of the array up to returned value.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>The position of the last distinct value in the argument array.</returns>
        /// <remarks>The contents of the array remains unchanged; only the order of the elements is changed.</remarks>
        public static int RemoveDuplicates(int[] numbers)
        {
            // validate arguments
            if (numbers == null)
            {
                throw new ArgumentNullException(nameof(numbers));
            }

            if (numbers.Length == 0)
            {
                return -1;
            }

            if (numbers.Length == 1)
            {
                return 0;
            }

            Array.Sort(numbers);

            int current = 0;
            int fwd = 1;
            int seen = numbers[current];
            int temp;

            while (fwd < numbers.Length)
            {
                if (numbers[fwd] == seen)
                {
                    fwd++;
                    continue;
                }

                current++;
                temp = numbers[current];
                numbers[current] = seen = numbers[fwd];
                numbers[fwd] = temp;
                fwd++;
            }

            return current;
        }
    }
}
