using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace DecimalToString
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float[] F = [100_000.1234F, 1.23456789E38F, 123456789.0E30F, 9876.54321E+7F, 9876.54321E+1F, 9876.54321E+2F, 9876.54321E+5F];
            int i = 0;
            foreach (float f in F)
            {
                Console.WriteLine($"F {i++,3}: {f,40} => {ToStringInteger(f),40:>}");
            }

            Console.WriteLine();

            double[] D = [100_000_000.00012345, 1.2345678901234567890E308, 123456.789E-4];

            foreach (double d in D)
            {
                Console.WriteLine($"D {i++,3}: {d,40} => {ToStringInteger(d),40:>}");
            }

            decimal[] M = [100_000_000_000.000000123456M, 12345678901234567890.123456789M, 9_999_999_999_999_999_999_999_999_999M, 1E22M];

            foreach (decimal m in M)
            {
                Console.WriteLine($"M {i++,3}: {m,40} => {ToStringInteger(m),40:>}");
            }

        }
        static string ToStringInteger(float f)
        {
            string f_str = f.ToString("G10", CultureInfo.InvariantCulture);
            //string f_str = f.ToString("F", CultureInfo.InvariantCulture);

            return NumberStringSplit(f_str);
        }

        static string ToStringInteger(double d)
        {
            string d_str = d.ToString("G20", CultureInfo.InvariantCulture);
            //string d_str = d.ToString("F", CultureInfo.InvariantCulture);

            return NumberStringSplit(d_str);
        }

        static string ToStringInteger(decimal d)
        {
            return d.ToString(CultureInfo.InvariantCulture).Split(".")[0]; 
        }

        static string NumberStringSplit(string n_str)
        {
            string[] n_parts = n_str.Split(['e', 'E'], StringSplitOptions.RemoveEmptyEntries);
            int pwr10 = n_parts.Length > 1? Convert.ToInt32(n_parts[1]) : 0;

            string[] number_parts = n_parts[0].Split(['.'], StringSplitOptions.RemoveEmptyEntries);
            string integer_part = number_parts[0];
            string decimal_part = number_parts.Length > 1 ? number_parts[1] : "0";

            int i = 0;
            while (pwr10 > 0)
            {
                integer_part += i < decimal_part.Length ? decimal_part[i] : "0";
                pwr10--;
                i++;
            }

            return integer_part;
        }

         
    }
}
