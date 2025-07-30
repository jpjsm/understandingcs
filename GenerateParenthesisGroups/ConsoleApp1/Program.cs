using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static bool IsMatching(string text)
        {
            Stack<string> s = new Stack<string>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(')
                {
                    s.Push("1");
                }

                if (text[i] == ')')
                {
                    if (s.Count == 0  )
                    {
                        return false;
                    }

                    s.Pop();
                }
            }

            return s.Count == 0;
        }

        static List<string> GenerateGroups(int n)
        {
            List<string> result = new List<string>();

            if (n % 2 == 1) throw new ArgumentException("Needs to be an even number.");

            int upperlimit = 1 << n;
            for (int i = 0; i < upperlimit; i++)
            {
                int bitoncount = 0;
                int k = i;
                for (int j = 0; j < n; j++)
                {
                    if ((k & 1) == 1)
                    {
                        bitoncount++;
                    }

                    k >>= 1;
                }
                if (bitoncount == n / 2)
                {
                    k = i;
                    string r = string.Empty;
                    for (int j = 0; j < n; j++)
                    {
                        if ((k & 1) == 1)
                        {
                            r = ")" + r;
                        }
                        else
                        {
                            r = "(" + r;
                        }
                        k >>= 1;
                    }

                    if (IsMatching(r))
                        result.Add(r);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            List<string> L = GenerateGroups(30);
            TimeSpan elapsed = DateTime.Now - start;
            Console.WriteLine("Total elapsed time: {0:N3} milliseconds.", elapsed.TotalMilliseconds);
            Console.WriteLine("Total combinations:: {0:N0} ", L.Count);

            //foreach(string l in L)
            //{
            //    Console.WriteLine(l);
            //}
        }
    }
}
