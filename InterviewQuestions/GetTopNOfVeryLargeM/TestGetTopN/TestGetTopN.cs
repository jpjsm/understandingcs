using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTopNOfVeryLargeM
{
    class TestGetTopN
    {

        static void Main(string[] args)
        {
            TopN<RankableItem> top10 = new TopN<RankableItem>(10);

            for (int i = -500; i < 500; i++)
            {
                for (int j = -499; j <=500; j++)
                {
                    top10.Scan(new RankableItem(i, j));
                }
            }


            RankableItem[] best10 = top10.Top;
            for (int i = 0; i < best10.Length; i++)
            {
                Console.WriteLine("Top[{0}]: Id={1}, X={2,4:D}, Y={3,4:D}, distance={4,12:N3} ", i, best10[i].Id, best10[i].X, best10[i].Y, Utils.Distance(best10[i].X, best10[i].Y));
            }
        }
    }
}
