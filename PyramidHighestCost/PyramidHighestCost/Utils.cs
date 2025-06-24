using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyramidHighestCost
{
    public static class Utils
    {
        public static Dictionary<int, int> MaxCost = new Dictionary<int, int>();

        public static int HighestCost(Node n)
        {
            if (MaxCost.ContainsKey(n.Id)) return MaxCost[n.Id];

            int L_Value = n.Left != null ? HighestCost(n.Left) : 0;
            int R_Value = n.Right != null ? HighestCost(n.Right) : 0;

            int max = n.Value + (L_Value > R_Value ? L_Value : R_Value);

            MaxCost.Add(n.Id, max);
            return max;
        }
    }
}
