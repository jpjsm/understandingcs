using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateBalancedParentheses
{
    public static class Utils
    {
        private static Dictionary<int, List<string>> GeneratedBalancedParentheses = new Dictionary<int, List<string>>();
        /// <summary>
        /// Generates all combinations of 'n' paired parentheses
        /// </summary>
        /// <param name="n">the number of paired parentheses</param>
        /// <returns>List of all possible combinations of paired parentheses.</returns>
        public static List<string> GenerateBalancedParentheses(int n)
        {
            if (n < 0) throw new ArgumentException("'n' must be equal or greater than zero (0).");

            if (GeneratedBalancedParentheses.ContainsKey(n))
                return GeneratedBalancedParentheses[n];

            List<string> result = new List<string>();
            if (n == 0)
            {
                result.Add(string.Empty);
                GeneratedBalancedParentheses.Add(n, result);
                return result;
            }

            for (int i = 0; i < n; i++)
            {
                foreach (string l in GenerateBalancedParentheses(i))
                {
                    foreach (string r in GenerateBalancedParentheses(n-1-i))
                    {
                        result.Add(string.Format("({0}){1}", l, r));
                    }
                }
            }

            GeneratedBalancedParentheses.Add(n, result);
            return result;
        }
    }
}
