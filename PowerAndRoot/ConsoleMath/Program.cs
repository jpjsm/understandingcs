using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMath
{
    using PowerAndRoot;

    class Program
    {
        static void Main(string[] args)
        {
            bool showDetails = false;
            if (args != null && args.Length > 1)
            {
                if (args[1] == "-ShowDetails")
                {
                    showDetails = true;
                }
            }

            long[] N = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
            long[] N5 = { 1048576, 1419857, 1889568, 2476099, 3200000, 4084101, 5153632, 6436343, 7962624, 9765625, 11881376, 14348907, 17210368, 20511149, 24300000, 28629151, 33554432 };
            int j = 0;
            int k = 0;
            DateTime stopWatch, fullStopWatch = DateTime.Now;
            TimeSpan lap;
            long actual;
            for (int i = 1048576; i < 33554432; i++)
            {
                if (i >= N5[j + 1])
                {
                    j++;
                }

                long expected = N[j];
                if (showDetails) Console.Write("Evaluating: {0:N0} == Root({1:N0}, 5). ", expected, i);
                stopWatch = DateTime.Now;
                actual = Math.Root(i, 5);
                lap = DateTime.Now - stopWatch;
                if (actual != expected)
                {
                    throw new ApplicationException(string.Format("Expected value '{0:N0}' != actual '{1:N0}', for i = {2:N0}", expected, actual, i));
                }

                if (showDetails) Console.WriteLine(" In {0:N6} millisecs", lap.TotalMilliseconds);
                k++;
            }

            lap = DateTime.Now - fullStopWatch;

            Console.WriteLine("{0:N0} fifth roots calculated in {1:N3} millisecs", k, lap.TotalMilliseconds);
        }
    }
}
