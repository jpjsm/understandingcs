namespace AnySizeIntegerTruncation;

class Program
{
    static void Main(string[] args)
    {
        (string, long)[] L = [
            ("long.MinValue",long.MinValue),
            ("long.MinValue + 1L", long.MinValue + 1L),
            ("int.MinValue - 1L", int.MinValue - 1L),
            ("int.MinValue", int.MinValue),
            ("int.MinValue + 1L", int.MinValue + 1L),

        ];

        foreach (var (s,n) in L)
        {
            unchecked
            {
                Console.WriteLine($"{s,-32}: {n,32:N0} | {Convert.ToString(n,2),64} | {(byte)n,20:N0}b | {(sbyte)n,20:N0}sb");
            }
        }


    }
}
