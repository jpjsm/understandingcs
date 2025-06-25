namespace IntHash
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int fooLength = 64;
            int[] foo = new int[fooLength];
            for (int i = 0; i < fooLength; i++)
            {
                foo[i] = fooLength - i -1;
            }

            int[] bar = new int[fooLength];
            int[] boo = new int[fooLength]; 
            foo.CopyTo(boo, 0);

            int[] exchanges = new int[] {8, 1, 34, 2, 21,  3, 13, 5};
            for (int i = 0; i < exchanges.Length; i++)
            {
                foo.CopyTo(bar, 0);
                Utils.SwapElements(ref bar, exchanges[i]);
                Utils.SwapElements(ref boo, exchanges[i]);

                foreach (int[] item in new int[][]{ bar, boo})
                {
                    Console.Write($"{nameof(item)}: ");
                    for (int j = 0; j < fooLength; j++)
                    {
                        Console.Write($"{item[j],3}");
                    }

                    Console.WriteLine();
                }
            }

            Environment.Exit(0);

            // TL; DR
            Dictionary<int,List<int>> hashes = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> flips = new Dictionary<int, List<int>>();
            char[] symbols = new char[] { '-', '\\', '|', '/' };
            long iteration = 1L;

            for (long i = int.MinValue; i <= int.MaxValue; i++)
            {
                int n = (int)i;
                int flip = n.Flip1().Flip2().Flip4().Flip8().Flip16();
                if (!flips.ContainsKey(flip))
                {
                    flips.Add(flip, new List<int>());
                }

                flips[flip].Add(n);

                if (flips[flip].Count > 1)
                {
                    Console.Write($"                            Colision flips[{flip:20N}] --> <{String.Join(", ", flips[flip].Select(f => f.ToString("N")))}>\r");
                }

                int hash = n.GetHashCode();
                if (!hashes.ContainsKey(hash))
                {
                    hashes.Add(hash, new List<int>());
                }

                hashes[hash].Add(n);

                if (hashes[hash].Count > 1)
                {
                    Console.Write($"                            Colision flips[{hash:20N}] --> <{String.Join(", ", hashes[hash].Select(f => f.ToString("N")))}>\r");
                }

                if ((iteration & 0b_1111_1111_1111_1111_1111) == 0)
                {
                    Console.Write($"    {iteration,20:N0}\r");
                }

                iteration++;

                Console.Write($"{symbols[(n < 0 ? -n : n) % symbols.Length]}\r");
            }

            Console.WriteLine("finished.");
        }
    }
}