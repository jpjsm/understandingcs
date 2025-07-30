using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTopNOfVeryLargeM
{
    public static class Utils
    {
        public static double Distance(long x, long y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }
        public static double Distance(ulong x, ulong y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }
    }
}
