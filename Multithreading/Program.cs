using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;


namespace Multithreading
{
    public class DoSomething
    {
        private Guid _id;
        private string _name;

        public Guid Id => _id;
        public string Name => _name;
        public bool TerminateThread { get; set; } = false;

        public DoSomething()
        {
            _id = Guid.NewGuid();
            _name = _id.ToString();
        }

        public DoSomething(string name)
            : this()
        {
            _name = name;
        }

        public void ThreadExecution()
        {
            Random rand = new Random();
            Console.WriteLine($"Thread: {_name} [id: {_id}");
            while (!TerminateThread)
            {
                BigInteger n1 = NewRandomBigInteger(500, 1000);
                BigInteger n2 = NewRandomBigInteger(500, 1000);
                BigInteger r = n1 * n2;
                File.WriteAllText($"BigNumbers-{_name}-{Id}.txt", $"{n1} * {n2} = {r}{Environment.NewLine}");
                Thread.Sleep(rand.Next(301, 601));                
            }
        }

        public static BigInteger NewRandomBigInteger(int minSizeBytes, int maxSizeBytes)
        {
            if (minSizeBytes < 1 || maxSizeBytes < 1)
            {
                throw new ArgumentException($"Both arguments must be positive numbers, greater than zero. {nameof(minSizeBytes)} == {minSizeBytes}; {nameof(maxSizeBytes)} == {maxSizeBytes}.");
            }

            Random rand = new Random();
            int size = rand.Next(minSizeBytes, maxSizeBytes);
            byte[] n_array = new byte[size];
            rand.NextBytes(n_array);

            return new BigInteger(n_array);
}

    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            Console.Write("Enter the number of threads this process has:");
            string initial_threads = Console.ReadLine();
            int i = 0;
            Thread theThread = null;

            while (true)
            {
                i++;
                string theThreadName = $"Thread {i}";
                ReplaceThread(ref theThread, theThreadName);
                Console.WriteLine($"Replaced '{theThreadName}'");
                Thread.Sleep(rand.Next(100, 200));
                theThread.Abort();
            }
        }

        static void ReplaceThread(ref Thread t, string name)
        {
            t = new Thread(new ThreadStart((new DoSomething(name)).ThreadExecution));
            t.IsBackground = true;
            t.Start();
        }
    }
}
