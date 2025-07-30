using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateBalancedParentheses
{
    class Program
    {
        static void Main(string[] args)
        {
            //int runs = 10;
            DateTime start = DateTime.Now;
            List<string> ParenthesesCombinations = GenerateBalancedParentheses.Utils.GenerateBalancedParentheses(15);
            //for (int i = 0; i < runs; i++)
            //{
            //    ParenthesesCombinations = GenerateBalancedParentheses.Utils.GenerateBalancedParentheses(10);
            //}

            TimeSpan elapsed = DateTime.Now - start;
            //foreach (string  c in ParenthesesCombinations)
            //{
            //    Console.WriteLine(c);
            //}
            DateTime start2 = DateTime.Now;
            List<string> ParenthesesCombinations2 = GenerateBalancedParentheses.Utils.GenerateBalancedParentheses(15);
            TimeSpan elapsed2 = DateTime.Now - start2;

            Console.WriteLine("Total milliseconds: {0:N3}", elapsed.TotalMilliseconds);
            Console.WriteLine("Total solutions: {0:N0}", ParenthesesCombinations.Count);

            Console.WriteLine("Total milliseconds: {0:N3}", elapsed2.TotalMilliseconds);
            Console.WriteLine("Total solutions: {0:N0}", ParenthesesCombinations2.Count);
        }
    }
}
