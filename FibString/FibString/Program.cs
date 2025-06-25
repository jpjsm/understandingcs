using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibString
{
    class Program
    {
        public static string FibString(int n, string s0 = "a", string s1="bc")
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (n == 0) return s0;
            if (n == 1) return s1;

            int i = 2;
            StringBuilder r0 = new StringBuilder(s0);
            StringBuilder r1 = new StringBuilder(s1);
            StringBuilder r2 = new StringBuilder(s0.Length + s1.Length);

            while (i <= n)
            {
                r2 = r0;
                r2.EnsureCapacity(r0.Length + r1.Length);
                r2.Append(r1);
                r0 = r1;
                r1 = r2;
                i++;
            }

            return r2.ToString();
        }


        static void Main(string[] args)
        {
            for (int i = 100; i < 101; i++)
            {
                Console.WriteLine("{0,3} {1}", i, FibString(i));
            }
        }
    }
}
