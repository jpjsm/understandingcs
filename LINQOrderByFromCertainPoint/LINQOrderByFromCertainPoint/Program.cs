using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQOrderByFromCertainPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

            numbers = numbers.Take(5).Concat(numbers.Skip(5).OrderBy(n => n)).ToList();
        }
    }
}
