using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingShiftOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * << Operator (C# Reference)
             * 
             *
             * If the first operand is an int or uint (32-bit quantity), 
             * the shift count is given by the low-order five bits of the second operand. 
             * That is, the actual shift count is 0 to 31 bits.
             * If the first operand is a long or ulong (64-bit quantity), 
             * the shift count is given by the low-order six bits of the second operand. 
             * That is, the actual shift count is 0 to 63 bits.
             * Any high-order bits that are not within the range of the type of the 
             * first operand after the shift are discarded, 
             * and the low-order empty bits are zero-filled. 
             * Shift operations never cause overflows.
             *
             * https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/left-shift-operator
            */
            long n = 0x8000;
            Console.WriteLine("n = {0:x}", n);

            while (n > 0)
            {
                n <<= -1; // is equivalent to <<= 31

                Console.WriteLine("n = {0:X}", n);
            }
        }
    }
}
