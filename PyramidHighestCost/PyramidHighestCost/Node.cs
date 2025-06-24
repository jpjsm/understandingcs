using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyramidHighestCost
{
    public class Node
    {
        private static int LastId = 0;

        public int Id { get; }
        public int Value { get;  }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(int v)
        {
            Id = LastId++;
            Value = v;
            Left = null;
            Right = null;
        }

        public Node(int v, Node l, Node r)
        {
            Id = LastId++;
            Value = v;
            Left = l;
            Right = r;
        }
    }
}
