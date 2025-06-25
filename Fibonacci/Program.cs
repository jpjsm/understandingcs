using System;
using System.Diagnostics;

namespace fibonacci;
class Program
{
    static long FibonacciRecursive(long n)
    {
        if (n < 0)
        {
            throw new ArgumentException($"'n' must be positive or zero.");
        }

        if (n == 0 || n == 1)
        {
            return 1;
        }

        return FibonacciRecursive(n-1) + FibonacciRecursive(n-2);
    }

    static long FibonacciIterative(long n)
    {
        if (n < 0)
        {
            throw new ArgumentException($"'n' must be positive or zero.");
        }

        if (n == 0 || n == 1)
        {
            return 1;
        }

        long[] fibs = new long[n+1];
        fibs[0] = 1L;
        fibs[1] = 1L;

        for (int i = 2; i <= n; i++)
        {
            fibs[i] = fibs[i-1] + fibs[i-2];
        }

        return fibs[n];
    }
    static T TimeIt<T>(T argument, Func<T, T> function, out double milliseconds)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        T result = function(argument);
        stopwatch.Stop();
        milliseconds = stopwatch.Elapsed.TotalMilliseconds;
        // Console.WriteLine($"\t-->Accuracy: {accuracy}, Ticks: {ticks:N0}, Nanoseconds : {nanoseconds:N0}, Microseconds: {microseconds:N0}, Milliseconds: {stopwatch.Elapsed.TotalMilliseconds:N3}, {stopwatch.Elapsed.TotalSeconds:N3}");

        return result;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Evaluate Fibonacci algorithms");
        Console.Write("... perss ENTER to continue ...");
        Console.ReadLine();
        Console.WriteLine($"Frequency: {Stopwatch.Frequency:N3}");
        Console.WriteLine($"Timer accuracy: {1000000000L/Stopwatch.Frequency:N2} nanoseconds");
        Console.WriteLine("\n===========================================================================================\n");

        // Discard first call
        TimeIt(10L, FibonacciRecursive, out double durationR);
        TimeIt(10L, FibonacciRecursive, out double durationI);

        Console.WriteLine($"  N |                  Fib Recursive |                  Fib Iterative | Recursive msecs | Iterative msecs |");
        Console.WriteLine( "----+--------------------------------+--------------------------------+-----------------+-----------------|"); ;

        for (long i = 0L; i <= 100L; i++)
        {
            long fibR = TimeIt<long>(i, FibonacciRecursive, out durationR);
            long fibI = TimeIt<long>(i, FibonacciIterative, out durationI);

            Console.WriteLine($"{i,3} | {fibR,30:N0} | {fibI,30:N0} | {durationR,15:N3} | {durationI,15:N3} |");
        }

        Console.WriteLine("Done !!");
    }
}
