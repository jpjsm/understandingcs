using System;

namespace NestedTryCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            double denominator = 0.0;

            try
            {
                Console.WriteLine("Hello World!");
                int retrytimes = 3;

            label1:
                try
                {
                    System.IO.File.OpenRead("C:\\foobar.txt");
                    Console.WriteLine("Found: C:\\foobar.txt");

                }
                catch (System.IO.FileNotFoundException fnfx)
                {
                    if (retrytimes > 0)
                    {
                        Console.WriteLine("Not found yet; retries left {0}", retrytimes);
                        retrytimes--;
                        goto label1;
                    }

                    Console.WriteLine("{0} {1}",fnfx.Message, fnfx.FileName);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Un-Expected {0}", e.Message);
                    throw;
                }
            }
            catch (Exception outerx)
            {
                try
                {
                    Console.WriteLine("About to divide by zero.");
                    double inverse = 1 / denominator;
                }
                catch
                {
                    Console.WriteLine("Outer exception: {0}", outerx.Message);
                    Console.WriteLine("Inner exception captured");
                    throw;
                }

                Console.WriteLine("About to exit with exception");
                throw;
            }

            Console.WriteLine("Done!");
        }
    }
}
