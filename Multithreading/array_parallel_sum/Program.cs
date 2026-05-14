/// ***************************************************************************
/// 
/// This code intents to shows how to convert a sequential process into a fully
/// parallelized process 
/// 
/// ***************************************************************************

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

    /// <summary>
    /// Adds the value of the j-th item to the value of the i-th item
    /// </summary>
    /// <param name="A">The array of values to add</param>
    /// <param name="i">the index of the value to be incremente by the j-th value</param>
    /// <param name="j">the index of the value to be added</param>
    /// <exception cref="ArgumentException">traps the invalid argumernts for debugging</exception>
    /// <remarks>By design: if j is outside the valid range of values, no operation is made</remarks>
    static void ArrayAdd(long[] A, long i, long j)
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

        long size = A.Length;
        long stride = 1;
        long step = 2;

        while (stride < size)
        {
            for (long j = 0; j < size; j += step)
                ArrayAdd(A, j, j + stride);
            stride <<= 1;
            step <<= 1;
        }

    }

    static void ParallelReductionArraySum(long[] A)
    {
        // Argument validation
        if (A == null || A.Length == 0) throw new ArgumentException("'A' must have elements");

        long size = A.Length;
        long stride = 1;
        long step = 2;

        while (stride < size)
        {
            Parallel.For(0, size / step + 1L, new ParallelOptions { MaxDegreeOfParallelism = 14 }, idx =>
            {
                long j = idx * step;
                ArrayAdd(A, j, j + stride);
            });
            stride <<= 1;
            step <<= 1;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine($"{"Size",16} | {"Serial (ms)",24} | {"Reduction-serial (ms),24"} | {"Reduction-parallel (ms),24"}");
        long[] sizes = [1031 * 1031, 5003 * 5003, 7001 * 7001, 10007 * 10007, 14143 * 14143, 17327 * 17327, 19997 * 19997, 22367 * 22367, 43331 * 43331];
        foreach (long size in sizes)
        {
            long[] A = new long[size];

            InitArray(A);
            long seq_array_sum = Profile(() => SequentialArraySum(A));

            InitArray(A);
            long seq_reduction_sum = Profile(() => SequentialReductionArraySum(A));


            InitArray(A);
            long par_reduction_sum = Profile(() => ParallelReductionArraySum(A));
            Console.WriteLine($"{size,16:N0} | {seq_array_sum,24:N0} | {seq_reduction_sum,24:N0} | {par_reduction_sum,24:N0}");
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
