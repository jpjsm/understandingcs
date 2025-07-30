using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyramidHighestCost
{
    class Program
    {
        static void Main(string[] args)
        {
            Node[] L4 = { new Node(21), new Node(14), new Node(-92), new Node(33) };
            Node[] L3 = { new Node(-4, L4[0], L4[1]), new Node(13, L4[1], L4[2]), new Node(45, L4[2], L4[3]) };
            Node[] L2 = { new Node(42, L3[0], L3[1]), new Node(-15, L3[1], L3[2]) };
            Node[] L1 = { new Node(137, L2[0], L2[1]) };

            Node top = L1[0];

            Console.WriteLine("The Highest cost: {0:N0}", Utils.HighestCost(top));
        }
    }
}
