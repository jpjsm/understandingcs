using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentsByReference
{
    class Program
    {
        static void Main(string[] args)
        {
            Something s1 = new Something("S1", 101, true);
            Something s2 = new Something("S2", 202, false);

            Console.WriteLine("s1: {0} | s2: {1}", s1, s2);

            NoSwap(s1, s2);
            Console.WriteLine("s1: {0} | s2: {1}", s1, s2);

            TryFullReplaceSomething(s1);
            TryFullReplaceSomething(s2);
            Console.WriteLine("s1: {0} | s2: {1}", s1, s2);

            UpdateSomethingName(s1, "Nueva Z1");
            Console.WriteLine("s1: {0} | s2: {1}", s1, s2);

            Swap(ref s1, ref s2);
            Console.WriteLine("s1: {0} | s2: {1}", s1, s2);
        }

        static void Swap(ref Something a, ref Something b)
        {
            Something t = a;
            a = b;
            b = t;
        }

        static void NoSwap(Something a, Something b)
        {
            Something t = a;
            a = b;
            b = t;
        }

        static void UpdateSomethingName(Something s, string newname)
        {
            s.Name = newname;
        }

        static void TryFullReplaceSomething(Something s)
        {
            s = new Something(Guid.NewGuid().ToString(), DateTime.Now.Millisecond, false);
        }
    }

    class Something
    {
        string name;
        int quantity;
        bool isworking;

        public Something(string n, int v, bool w)
        {
            name = n;
            quantity = v;
            isworking = w;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public bool IsWorking
        {
            get { return isworking; }
            set { isworking = value; }
        }

        public override string ToString()
        {
            return string.Format("'{0}', {1}, {2}", name, quantity, isworking);
        }
    }
}
