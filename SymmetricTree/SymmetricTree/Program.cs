using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymmetricTree
{
    public static class Utils
    {
        public static bool IsPalindrome<T>(this List<T> l) where T : IComparable
        {
            int half = l.Count / 2;
            for (int i = 0; i < half; i++)
            {
                if (l[i].CompareTo(l[l.Count - 1 - i]) != 0) return false;
            }

            return true;
        }

        public static  bool IsOdd(this int n)
        {
            return (n & 1) == 1;
        }

        public static bool IsOdd(this uint n)
        {
            return (n & 1) == 1U;
        }
    }

    public class Node<T> where T : IComparable
    {
        public T data;
        public List<Node<T>> nodes;

        public Node(T d)
        {
            data = d;
            nodes = new List<Node<T>>();
        }

        public void InsertFront(Node<T> n)
        {
            nodes.Insert(0, n);
        }

        public void InsertEnd(Node<T> n)
        {
            nodes.Add(n);
        }

        public void InsertAt(int i, Node<T> n)
        {
            nodes.Insert(i, n);
        }

        public List<T> LeftFirst()
        {
            Queue<T> result = new Queue<T>();

            for (int i = 0; i < nodes.Count; i++)
            {
                List<T> s = nodes[i].LeftFirst();
                for (int j = 0; j < s.Count; j++)
                {
                    result.Enqueue(s[j]);
                }
            }

            result.Enqueue(data);
            return result.ToList<T>();
        }

        public List<T> RightFirst()
        {
            Stack<T> result = new Stack<T>();

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                List<T> s = nodes[i].RightFirst();
                for (int j = s.Count - 1; j >= 0; j--)
                {
                    result.Push(s[j]);
                }
            }

            result.Push(data);
            return result.ToList<T>();
        }

        public List<T> GetLeft()
        {
            List<T> result = new List<T>();

            if (nodes.Count < 2)
            {
                return result;
            }

            int half = nodes.Count / 2;
            for (int i = 0; i < half; i++)
            {
                result.AddRange(nodes[i].LeftFirst());
            }

            return result;
        }

        public List<T> GetRight()
        {
            List<T> result = new List<T>();

            if (nodes.Count < 2)
            {
                return result;
            }

            int half = (nodes.Count / 2) + (nodes.Count.IsOdd() ? 1 : 0);

            for (int i = half; i < nodes.Count; i++)
            {
                result.AddRange(nodes[i].RightFirst());
            }

            return result;
        }

        public bool IsSymmetric()
        {
            /* Short circuit version
            return (nodes.Count.IsOdd() ? nodes[nodes.Count / 2].IsSymmetric() : true) &&
                GetLeft().Concat(GetRight()).ToList<T>().IsPalindrome<T>();
            */

            bool SymmetricMiddle = true;
            if (nodes.Count.IsOdd())
            {
                int n2 = nodes.Count / 2;
                SymmetricMiddle = nodes[n2].IsSymmetric();
            }

            return SymmetricMiddle && 
                   GetLeft().Concat(GetRight()).ToList<T>().IsPalindrome<T>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<int> capicua = new List<int> { 1, 2, 3, 4, 5, 4, 3, 2, 1 };
            Console.WriteLine("Es capicua {0}", capicua.IsPalindrome());

            List<int> nocapicua = new List<int> { 1, 2, 3, 4, 5, 6, 3, 2, 1 };
            Console.WriteLine("Es nocapicua {0}", !nocapicua.IsPalindrome());

            List<string> Reversible = new List<string> { "Hola", "Mundo", "Divertido", "Mundo", "Hola" };
            Console.WriteLine("Es reversible {0}", Reversible.IsPalindrome());

            List<string> NoReversible = new List<string> { "Hola", "Mundo", "Divertido", "y", "Amable", "Mundo", "Hola" };
            Console.WriteLine("Es no-reversible {0}", !NoReversible.IsPalindrome());

            Node<int> root = new Node<int>(0);

            // Leftmost node ==> node[0]
            root.InsertEnd(new Node<int>(1));
            root.nodes[0].InsertEnd(new Node<int>(11));
            root.nodes[0].InsertEnd(new Node<int>(12));
            root.nodes[0].nodes[1].InsertEnd(new Node<int>(121));
            root.nodes[0].nodes[1].nodes[0].InsertEnd(new Node<int>(1211));
            root.nodes[0].nodes[1].nodes[0].InsertEnd(new Node<int>(1212));
            root.nodes[0].InsertEnd(new Node<int>(13));

            // Second leftmost node ==> node[1]
            root.InsertEnd(new Node<int>(2));
            root.nodes[1].InsertEnd(new Node<int>(21));
            root.nodes[1].InsertEnd(new Node<int>(22));
            root.nodes[1].nodes[1].InsertEnd(new Node<int>(221));
            root.nodes[1].nodes[1].nodes[0].InsertEnd(new Node<int>(2211));
            root.nodes[1].nodes[1].nodes[0].InsertEnd(new Node<int>(2212));
            root.nodes[1].nodes[1].InsertEnd(new Node<int>(222));

            // Middle node ==> node[2]
            root.InsertEnd(new Node<int>(3));
            root.nodes[2].InsertEnd(new Node<int>(31));
            root.nodes[2].nodes[0].InsertEnd(new Node<int>(311));
            root.nodes[2].nodes[0].InsertEnd(new Node<int>(312));
            root.nodes[2].InsertEnd(new Node<int>(31));
            root.nodes[2].nodes[1].InsertEnd(new Node<int>(312));
            root.nodes[2].nodes[1].InsertEnd(new Node<int>(311));

            // Second rightmost node ==> node[3]
            root.InsertEnd(new Node<int>(2));
            root.nodes[3].InsertEnd(new Node<int>(22));
            root.nodes[3].nodes[0].InsertEnd(new Node<int>(222));
            root.nodes[3].nodes[0].InsertEnd(new Node<int>(221));
            root.nodes[3].nodes[0].nodes[1].InsertEnd(new Node<int>(2212));
            root.nodes[3].nodes[0].nodes[1].InsertEnd(new Node<int>(2211));
            root.nodes[3].InsertEnd(new Node<int>(21));

            // Rightmost node ==> node[4]
            root.InsertEnd(new Node<int>(1));
            root.nodes[4].InsertEnd(new Node<int>(13));
            root.nodes[4].InsertEnd(new Node<int>(12));
            root.nodes[4].nodes[1].InsertEnd(new Node<int>(121));
            root.nodes[4].nodes[1].nodes[0].InsertEnd(new Node<int>(1212));
            root.nodes[4].nodes[1].nodes[0].InsertEnd(new Node<int>(1211));
            root.nodes[4].InsertEnd(new Node<int>(11));

            foreach (int d in root.GetLeft())
            {
                Console.Write("{0,5:D}", d);
            }

            Console.WriteLine();


            foreach (int d in root.GetRight())
            {
                Console.Write("{0,5:D}", d);
            }

            Console.WriteLine();

            Console.WriteLine("Is tree summetric {0}", root.IsSymmetric());
        }
    }
}
