using System;
using System.Linq;

namespace PermutationsKnuthsSolution
{
    class BinaryString8Permuter
    {
        protected virtual void Visit(byte a)
        {
            for (int i = 7; i >= 0; i--)
            {
                System.Console.Write((a >> i) & 1);
            }
            System.Console.WriteLine();
        }

        public void VisitAll()
        {
            byte a = 0;
            do
            {
                Visit(a);
            } while ((++a) != 0);
        }
    }

    class IntNTuplePermuter
    {
        protected virtual void Visit(int[] a)
        {
            string head = "";
            for (int j = 1; j < a.Length; j++)
            {
                System.Console.Write("{0}{1}", head, a[j]);
                head = ", ";
            }
            System.Console.WriteLine();
        }

        public void VisitAll(int m, int n)
        {
            // Initialize. Because C# automatically initializes
            // integers to 0, we need not explictely do so.
            // We allocate n + 1 integers to make room for a0.
            int j;
            int[] a = new int[n + 1];

            for (; ; )
            {
                Visit(a);

                // Prepare to add one.
                j = n;

                // Carry if necessary.
                while (a[j] == (m - 1))
                {
                    a[j] = 0;
                    j -= 1;
                }

                // Increase unless done.
                if (j == 0)
                {
                    break; // Terminate the algorithm
                }
                else
                {
                    a[j] = a[j] + 1;
                }
            }
        }
    }

    class EnumerablePermuter
    {
        protected virtual void Visit(System.Collections.IEnumerator[] a)
        {
            string head = "";
            for (int j = 1; j < a.Length; j++)
            {
                System.Console.Write("{0}{1}", head, a[j].Current);
                head = " ";
            }
            System.Console.WriteLine();
        }

        public void VisitAll(params System.Collections.IEnumerable[] m)
        {
            // Initialize.
            int n = m.Length;
            int j;
            System.Collections.IEnumerator[] a =
                new System.Collections.IEnumerator[n + 1];

            for (j = 1; j <= n; j++)
            {
                a[j] = m[j - 1].GetEnumerator();
                a[j].MoveNext();
            }
            a[0] = m[0].GetEnumerator();
            a[0].MoveNext();

            for (; ; )
            {
                Visit(a);

                // Prepare to add one.
                j = n;

                // Carry if necessary.
                while (!a[j].MoveNext())
                {
                    a[j].Reset();
                    a[j].MoveNext();
                    j -= 1;
                }

                // Increase unless done.
                if (j == 0)
                {
                    break; // Terminate the algorithm
                }
            }
        }
    }

    class ArrayRotator
    {
        protected virtual void Swap(ref object a, ref object b)
        {
            object t = a;
            a = b;
            b = t;
        }

        protected virtual void Visit(object[] a)
        {
            System.Console.WriteLine(string.Join(", ", a.Select(o => o.ToString())));
        }

        public void VisitAll(params object[] m)
        {
            object[] a = new object[m.Length];
            m.CopyTo(a, 0);
            int i = 0;

            while(true)
            {
                Visit(a);
                Swap(ref a[i], ref a[i + 1]);
                i = ++i % (m.Length - 1);
                //if (a.Zip(m, (first, second) => first == second).All(t => t)) 
                //    break;
                if (a.Zip(m, (first, second) => first.Equals(second)).All(t => t))
                    break;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //(new BinaryString8Permuter()).VisitAll();
            //(new IntNTuplePermuter()).VisitAll(2, 3);

            //string[] herbs = { "bay", "oregano", "dill" };
            //string[] ingredients = { "beef", "prawns", "bacon", "sausage" };
            //double[] quantities = { 0.25, 0.5, 1.0, 2.0, 4.0 };
            //object[] everything = herbs.Concat(ingredients.Select(i => (object)i)).Concat(quantities.Select(q => (object)q)).ToArray();
            //(new EnumerablePermuter()).VisitAll(herbs.Take(2).ToArray(), ingredients.Take(2).ToArray(), quantities.Take(2).ToArray());
            //(new EnumerablePermuter()).VisitAll(everything, everything, everything);
            (new ArrayRotator()).VisitAll(0,0,0,0,1,1,1,1);
        }
    }
}
