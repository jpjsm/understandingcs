using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosestAncestor
{
    public class Node
    {
        public Node Left, Right;
        public int Value;

        public static Node ClosestAncestor(Node root, int a, int b)
        {
            if (root == null)
            {
                return null;
            }

            if ((root.Value >= a && root.Value <= b) ||
                (root.Value >= b && root.Value <= a))
            {
                return root;
            }

            if (root.Value > a && root.Value > b)
            {
                return ClosestAncestor(root.Left, a, b);
            }

            return ClosestAncestor(root.Right, a, b);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /* 
            *  ============================================================================
            * 
            *  Tree:
            *                                  20
            *                 /----------------^---------------\
            *                13                                 30
            *        /-------^---------\                   /----^--------\
            *        8                 16                 25             40
            *                        /--^--\                           /--^---\
            *                       14     18                         35      50
            *
            *  ============================================================================
            */
            Node tree = new Node()
            {
                Value = 20,
                Left = new Node()
                {
                    Value = 13,
                    Left = new Node()
                    {
                        Value = 8
                    },
                    Right = new Node()
                    {
                        Value = 16,
                        Left = new Node()
                        {
                            Value = 14
                        },
                        Right = new Node()
                        {
                            Value = 18
                        }
                    }
                },
                Right = new Node()
                {
                    Value = 30,
                    Left = new Node()
                    {
                        Value = 25
                    },
                    Right = new Node()
                    {
                        Value = 40,
                        Left = new Node()
                        {
                            Value = 35
                        },
                        Right = new Node()
                        {
                            Value = 50
                        }
                    }
                }
            };

            Node ca = Node.ClosestAncestor(tree, 19, 21);
            Debug.Assert(ca.Value == 20);

            ca = Node.ClosestAncestor(tree, 18, 14);
            Debug.Assert(ca.Value == 16);

            ca = Node.ClosestAncestor(tree, 30, 140);
            Debug.Assert(ca.Value == 30);

            ca = Node.ClosestAncestor(tree, 15, 16);
            Debug.Assert(ca.Value == 16);

            ca = Node.ClosestAncestor(tree, 9, 11);
            Debug.Assert(ca == null);
        }
    }
}
