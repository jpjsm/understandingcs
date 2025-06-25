using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingChars
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i <= UInt16.MaxValue; i++)
            {
                char c = Convert.ToChar(i);
                if (i % 40 == 0)
                {
                    Console.WriteLine();
                    Console.ReadKey(intercept: true);
                }
                Console.Write("{0,4}", c);
            }
        }
    }
}
