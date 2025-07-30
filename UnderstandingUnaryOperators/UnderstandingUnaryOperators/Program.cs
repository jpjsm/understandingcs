using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingUnaryOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            long a = 21L;
            long b = -a;
            long c = ~a;

            Console.WriteLine(" a = {0,4} ({1,64})", a, Convert.ToString(a, 2));
            Console.WriteLine("-a = {0,4} ({1,64})", b, Convert.ToString(b, 2));
            Console.WriteLine("~a = {0,4} ({1,64})", c, Convert.ToString(c, 2));

            /*
             * Convert.ToString Method (Int64, Int32)
             *
             * Converts the value of a 64-bit signed integer to its equivalent string representation in a specified base.
             * The base of the return value, which must be 2, 8, 10, or 16.
             * 
            */
        }
    }
}
