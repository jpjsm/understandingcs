using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqWithMultidimensionalArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] a = new int[3, 3];
            List<Tuple<int, int>> OddNumbers = new List<Tuple<int, int>>();
            List<Tuple<int, int>> Numbers = new List<Tuple<int, int>>();


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    a[i, j] = 3 * i + j;
                    Numbers.Add(new Tuple<int, int>(i, j));
                }
            }

            Array x = a;

            // Can't find a way to take rows as lists and do SelectMany

            // Using coordinates to iterate
            OddNumbers = Numbers.Where(t => a[t.Item1, t.Item2] % 2 == 1).ToList();
            List<Tuple<int, int>> NumbersDescending = Numbers.OrderByDescending(t => a[t.Item1, t.Item2]).ToList();
        }
    }
}
