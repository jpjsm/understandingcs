namespace TestImageOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ImageOperations;

    class Program
    {
        static int Main(string[] args)
        {
            int results = TestSwapQuadrants();
            if (results != 0)
            {
                Console.WriteLine("Errors in TestSwapQuadrants: {0}", results);
            }
            return results;
        }

        static int TestSwapQuadrants()
        {
            int results = 0;
            uint[] pix = { 0, 1, 2, 3 };

            // Test invalid arguments
            if (ImageOperations.SwapQuadrants(null, 32))
            {
                results |= 1;
            }

            if (ImageOperations.SwapQuadrants(pix, 32))
            {
                results |= 1 << 1;
            }

            if (ImageOperations.SwapQuadrants(pix, 1))
            {
                results |= 1 << 2;
            }

            if (ImageOperations.SwapQuadrants(pix, 3))
            {
                results |= 1 << 3;
            }

            if (ImageOperations.SwapQuadrants(pix, 4))
            {
                results |= 1 << 4;
            }
         
            // Test functionality
            if (results == 0)
            {
                /* Pix:  4 rows x 4 columns
                     0  1  2  3    \    10 11  8  9
                     4  5  6  7  ---\   14 15 12 13
                     8  9 10 11  ---/    2  3  0  1
                    12 13 14 15    /     6  7  4  5
                */
                pix = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                uint[] expected = { 10, 11, 8, 9, 14, 15, 12, 13, 2, 3, 0, 1, 6, 7, 4, 5 };
                int bitmaskStart = 5;

                results = ValidateSwap(results, pix, 4U, expected, bitmaskStart);

                /* Pix:  4 rows x 5 columns
                     0  1  2  3  4    \    12 13 14 10 11
                     5  6  7  8  9  ---\   17 18 19 15 16
                    10 11 12 13 14  ---/    2  3  4  0  1
                    15 16 17 18 19    /     7  8  9  5  6
                */
                pix = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                expected = new uint[] { 12, 13, 14, 10, 11, 17, 18, 19, 15, 16, 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 };
                bitmaskStart = 8;

                results = ValidateSwap(results, pix, 5U, expected, bitmaskStart);

                /* Pix:  5 rows x 4 columns
                     0  1  2  3    \    10 11  8  9
                     4  5  6  7  ---\   14 15 12 13
                     8  9 10 11  ---/   18 19 16 17
                    12 13 14 15    /     2  3  0  1
                    16 17 18 19          6  7  4  5
                */
                pix = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                expected = new uint[] { 10, 11, 8, 9, 14, 15, 12, 13, 18, 19, 16, 17, 2, 3, 0, 1, 6, 7, 4, 5 };
                bitmaskStart = 11;

                results = ValidateSwap(results, pix, 4U, expected, bitmaskStart);
            }

            return results;
        }

        private static int ValidateSwap(int results, uint[] pix, uint ppr, uint[] expected, int bitmaskStart)
        {
            if (ImageOperations.SwapQuadrants(pix, ppr))
            {
                if (pix.Length != expected.Length)
                {
                    results |= 1 << (bitmaskStart + 0);
                }
                else
                {
                    for (int i = 0; i < pix.Length; i++)
                    {
                        if (pix[i] != expected[i])
                        {
                            results |= 1 << (bitmaskStart + 1);
                            break;
                        }
                    }
                }
            }
            else
            {
                results |= 1 << (bitmaskStart + 2);
            }

            return results;
        }
    }
}
