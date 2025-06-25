using System;

namespace TriangularSquareNumbers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using NumberTheory;

    using Newtonsoft.Json;

    public class TriangularSquareNumber
    {
        public BigInteger N { get; private set; } 
        public BigInteger SquareBase { get; private set; }
        public BigInteger TriangularBase { get; private set; }
        public BigInteger GCD { get; private set; }
        public List<BigInteger> Factors { get; private set; }

        public TriangularSquareNumber(BigInteger n, BigInteger s, BigInteger t, BigInteger gcd, List<BigInteger> f)
        {
            N = n;
            SquareBase = s;
            TriangularBase = t;
            GCD = gcd;
            Factors = f;
        }
}


    public class Program
    {
        public static ConcurrentBag<TriangularSquareNumber> TSN = new ConcurrentBag<TriangularSquareNumber>(); 
        static void Main()
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount - 1;

            try
            {
                Parallel.For(
                        1L,
                        long.MaxValue,
                        (i) =>
                        {
                            CheckForTriangularSquareNumber(i);
                        }
                    );
            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException e)
            {
                Console.WriteLine("Parallel.For has thrown the following (unexpected) exception:\n{0}", e);
            }

            var trangularsquarenumbers = TSN.OrderBy(t => t.N).ToList();
            System.IO.File.WriteAllText($"TriangularSquareNumbers.json", JsonConvert.SerializeObject(trangularsquarenumbers));
        }
    
        public static bool CheckForTriangularSquareNumber(Int64 n)
        {
            BigInteger triangular_base = n;
            BigInteger Triangular = (triangular_base * (triangular_base + 1)) >> 1;
            BigInteger square_base = Triangular.Sqrt();
            BigInteger Square = square_base * square_base;

            if (Square == Triangular)
            {
                TriangularSquareNumber tsn = new TriangularSquareNumber(
                    Triangular, 
                    square_base, 
                    triangular_base, 
                    BigInteger.GreatestCommonDivisor(square_base, triangular_base), 
                    Triangular.PrimeFactors()
                    );
                TSN.Add(tsn);
                Console.WriteLine(Triangular.ToString("#,##0"));
                System.IO.File.WriteAllText($"TSN_{Triangular:0}.json", JsonConvert.SerializeObject(tsn));

                return true;
            }

            return false;
        }
    }

}
