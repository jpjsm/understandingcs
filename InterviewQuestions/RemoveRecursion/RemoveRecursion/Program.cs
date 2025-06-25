using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveRecursion
{
    class Program
    {
        public static List<int> buffer = new List<int>();
        static void Main(string[] args)
        {
            List<int> expected, results;
            for (int i = -1; i < 11; i++)
            {
                buffer = new List<int>();
                Mystery(i);
                expected = buffer;
                buffer = new List<int>();
                Mystery2(i);
                results = buffer;
                Debug.Assert(
                    expected.Count == results.Count,
                    string.Format(
                        "Test case: {0}. Lists lengths are different: {1} != {2}",
                        i,
                        expected.Count,
                        results.Count));
                for (int j = 0; j < expected.Count; j++)
                {
                    Debug.Assert(
                        expected[j] == results[j],
                        string.Format(
                            "Test case: {0}. elements at position {1} are different: {2} != {3}",
                            i,
                            j,
                            expected.Count,
                            results.Count));
                }

            }
        }

        public static void Print(int n)
        {
            buffer.Add(n);
        }

        public static void Mystery(int n)
        {
            Print(n);
            if (n > 0)
            {
                if ((n % 2) != 0)
                {
                    Mystery(n - 1);
                }
                else
                {
                    if ((n % 3) != 0)
                    {
                        Mystery(n - 2);
                    }
                }
            }
        }

        public static void Mystery2(int n)
        {
            begin: Print(n);
            if (n > 0)
            {
                if ((n % 2) != 0)
                {
                    n = n - 1;
                    goto begin;
                }
                else
                {
                    if ((n % 3) != 0)
                    {
                        n = n - 2;
                        goto begin;
                    }
                }
            }
        }

        public static void Mystery3(int n)
        {
            while (true)
            {
                Print(n);
                if (n > 0)
                {
                    if ((n % 2) != 0)
                    {
                        n = n - 1;
                        continue;
                    }
                    else
                    {
                        if ((n % 3) != 0)
                        {
                            n = n - 2;
                            continue;
                        }
                    }
                }

                break;
            }
        }
    }
}
