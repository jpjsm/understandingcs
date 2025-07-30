using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumCombinationsOfN
{
    public static class Utils
    {
        private static Dictionary<int, List<List<string>>> KnownSumCombinations = new Dictionary<int, List<List<string>>>();

        public static List<List<string>> GenerateSumCombinations(int n)
        {
            if (n < 0) throw new ArgumentException(string.Format("'{0}' must be greater than zero (0).", nameof(n)));

            if (n == 0)
            {
                return new List<List<string>> { new List<string> { string.Empty } };
            }

            List<List<string>> result = new List<List<string>>
            {
                new List<string> { n.ToString() }
            };

            HashSet<string> solutions = new HashSet<string>();

            for (int i = n - 1; i > 0; i--)
            {
                foreach (List<string> combination in GenerateSumCombinations(n - i))
                {
                    combination.Insert(0, i.ToString());
                    if (solutions.Add(string.Join("+", combination.OrderBy(s => Convert.ToInt32(s)))))
                    {
                        result.Add(combination.OrderByDescending(s => Convert.ToInt32(s)).ToList());
                    }
                }
            }

            return result;
        }
    }
}
