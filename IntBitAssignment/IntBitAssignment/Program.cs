using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntBitAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            uint A = 0xAAAAAAAA;

            int a = (int)A;

            uint B = (uint)a;

            if(A==B)
            {
                Console.WriteLine("Succeed");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
    }
}
