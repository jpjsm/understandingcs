using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumCombinationsOfN
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> Test1 = Utils.GenerateSumCombinations(16);
            foreach (List<string> t in Test1)
            {
                Console.WriteLine(string.Join(" + ", t.Where(s => !string.IsNullOrWhiteSpace(s))));
            }
        }

    }
}
