using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator
{
    public class WaveGenerator
    {
        public static int TriangleWavePoint(int x, int periodCount, int max)
        {
            x = x % periodCount;
            int halfperiod = periodCount >> 1;
            int max2 = max << 1;

            if (x < halfperiod)
            {
                return (x * max2) / periodCount;
            }

            return max2 - (x * max2) / periodCount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("  X |  Y");
            Console.WriteLine("----+----");

            for (int i = 0; i <= 32; i++)
            {
                Console.WriteLine("{0,3} |{1,3}", i, WaveGenerator.TriangleWavePoint(i, 16, 8));
            }

            Console.WriteLine("=========");
            Console.WriteLine("         ");
            Console.WriteLine("  X |  Y");
            Console.WriteLine("----+----");

            for (int i = 0; i <= 32; i++)
            {
                Console.WriteLine("{0,3} |{1,3}", i, WaveGenerator.TriangleWavePoint(i, 8, 8));
            }
        }

    }
}
