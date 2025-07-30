using System.Globalization;

namespace ReferencingStrings;

class Program
{
    class Foo
    {
        bool isNegative = false;
        ulong[] digits;

        public Foo(int n)
        {
            isNegative = n < 0;
            digits = new ulong[1];

            digits[0] = isNegative ? (ulong)-n : (ulong)n;
        }

        public override string ToString()
        {
            return isNegative ? "-" + digits[0].ToString() : digits[0].ToString();
        }
    }
    static void Main(string[] args)
    {
        Foo A1 = new Foo(int.MinValue);
        Foo B1 = A1;

        Console.WriteLine($"    A : {A1} <--> {B1} : B");
        A1 = new Foo(0);
        Console.WriteLine($"new A : {A1} <--> {B1} : B");
    }
}
