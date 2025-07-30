using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageOperations
{
    public static class ImageOperations
    {
        public static bool SwapQuadrants(uint[] pix, uint ppr)
        {
            // Validate arguments
            if (pix == null ||
                ppr < 2 ||
                (uint)pix.Length < ppr ||
                ((uint)pix.Length % ppr) != 0 ||
                ((uint)pix.Length / ppr) < 2)
            {
                return false;
            }


            // Create copy
            uint[] copy = new uint[pix.Length];

            // Swap
            long rows = (pix.LongLength / ppr);
            long left_half = ppr / 2;
            long right_half = ppr - left_half;
            long top_rows = rows / 2;
            long bottom_rows = rows - top_rows;

            long row, column, newrow, newcolumn;
            for (long i = 0; i < pix.Length; i++)
            {
                row = i / ppr;
                column = i % ppr;
                newcolumn =
                    column < left_half ?
                    column + right_half :
                    column - left_half;
                newrow =
                    row < top_rows ?
                    row + bottom_rows :
                    row - top_rows;

                copy[newrow * ppr + newcolumn] = pix[i];
            }

            // Copy back to pix
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = copy[i];
            }

            return true;
        }
    }
}
