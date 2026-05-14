using System.Diagnostics;


namespace array_parallel_sum;

public static class Profiler
{
    public static long Measure(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
}


class Program
{
    public static long Profile(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    static void InitArray(long[] A)
    {
        // Argument validation
        if (A == null || A.Length == 0) throw new ArgumentException("'A' must have elements");

        for (int i = 0; i < A.Length; i++)
            A[i] = i;
    }
    static void ArrayAdd(long[] A, int i, int j)
    {
        // Argument validation
        if (A == null || A.Length == 0)
        {
            throw new ArgumentException("'A' must have elements");
        }

        if (i >= A.Length)
        {
            throw new ArgumentException("'i' must be a valid index in the array");
        }

        if (i >= j)
        {
            throw new ArgumentException("'i' must less than 'j'");
        }

        if (j < A.Length)
        {
            try
            {
                A[i] += A[j]; // For 'j' outside the array, leave A[i] unchanged
            }
            catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine($"A Length: {A.Length:N0}, i: {i:N0}, j: {j:N0}");
                throw;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        return; // obvious, but cleaner.
    }

    static void SequentialArraySum(long[] A)
    {
        // Argument validation
        if (A == null || A.Length == 0) throw new ArgumentException("'A' must have elements");

        for (int i = 1; i < A.Length; i++)
            A[0] += A[i];
    }

    static void SequentialReductionArraySum(long[] A)
    {
        // Argument validation
        if (A == null || A.Length == 0) throw new ArgumentException("'A' must have elements");

        int size = A.Length;
        int stride = 1;
        int step = 2;

        while (stride < size)
        {
            for (int j = 0; j < size; j += step)
                ArrayAdd(A, j, j + stride);
            stride <<= 1;
            step <<= 1;
        }

    }

    static void ParallelReductionArraySum(long[] A)
    {
        // Argument validation
        if (A == null || A.Length == 0) throw new ArgumentException("'A' must have elements");

        int size = A.Length;
        int stride = 1;
        int step = 2;

        while (stride < size)
        {
            Parallel.For(0, size / step + 1, new ParallelOptions { MaxDegreeOfParallelism = 256 }, idx =>
            {
                int j = idx * step;
                ArrayAdd(A, j, j + stride);
            });
            stride <<= 1;
            step <<= 1;
        }
    }

    static void Main(string[] args)
    {
        long[] sizes = [1031 * 1031, 5003 * 5003, 7001 * 7001, 10007 * 10007, 14143 * 14143, 17327 * 17327, 19997 * 19997, 22367 * 22367, 43331 * 43331];
        foreach (long size in sizes)
        {
            long[] A = new long[size];

            InitArray(A);
            Console.WriteLine("Size: {0:N0}, SequentialArraySum: {1} ms", size, Profile(() => SequentialArraySum(A)));

            InitArray(A);
            Console.WriteLine("Size: {0:N0}, SequentialReductionArraySum: {1} ms", size, Profile(() => SequentialReductionArraySum(A)));


            InitArray(A);
            Console.WriteLine("Size: {0:N0}, ParallelReductionArraySum: {1} ms", size, Profile(() => ParallelReductionArraySum(A)));
        }
        // foreach(int base in bases)
        // {

        // }
        // int size = 5003 * 5003;

        // long actual = A[0];
        // long expected = (long)size * ((long)size - 1) / 2;

        // Console.WriteLine("Actual   sum: {0:N0}", actual);
        // Console.WriteLine("Expected sum: {0:N0}", expected);
        // Console.WriteLine("Is match: {0}", actual == expected);

    }
}
