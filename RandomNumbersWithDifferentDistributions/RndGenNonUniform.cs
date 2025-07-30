using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

using RandomNumbersWithDifferentDistributions;

namespace Test_RandomNumbersWithDifferentDistributions
{
    public class Application
    {
        public static void Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, Test RandomNumbersWithDifferentDistributions!");

            RandomUSqrt rndusqrt = new();
            int bins_number = 31;
            int iterations = 100000;
            int[] bins = new int[bins_number];
            for (int i = 0; i < bins_number; i++) bins[i] = 0;
            double gauge = 1.0/(double)bins_number;
            for (int i = 0; i < iterations; i++)
            {
                double nextrnd = rndusqrt.NextDouble();
                double bin_approx = nextrnd / gauge;
                int bin_index = (int)bin_approx;
                bins[bin_index] += 1;
            }

            Console.Write(new string(' ', 8));
            for (int i = 0; i < 11; i++)
            {
                Console.Write($"{i%10}----+----");
            }
            Console.WriteLine();

            for (int i = 0; i < bins_number; i++)
            {
                int height = (int) (800.0 * ((double) bins[i]/ (double)iterations));
                Console.WriteLine($"{i*gauge:f4} +{new string('*', height)}");
            }

            Console.WriteLine("Goodbye, Test RandomNumbersWithDifferentDistributions!");
        }
    }

}
