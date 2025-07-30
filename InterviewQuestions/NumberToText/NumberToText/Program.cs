using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberToText
{
    class Program
    {
        public static long[] breaks = { 1000000000L, 1000000L, 1000L, 100L };
        public static string[] breakLabels = { "billion", "million", "thousand", "hundred" };
        public static string[] tens = { "", "ten", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        public static string[] teens = { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        public static string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        static void Main(string[] args)
        {
            Tuple<double, string>[] unitTests = new Tuple<double, string>[] 
                {
                    new Tuple<double, string>(.34, "zero dollars and 34 cents"),
                    new Tuple<double, string>(6.0, "six dollars and 00 cents"),
                    new Tuple<double, string>(19.57, "nineteen dollars and 57 cents"),
                    new Tuple<double, string>(62.2134, "sixty two dollars and 21 cents"),
                    new Tuple<double, string>(999.34, "nine hundred ninety nine dollars and 34 cents"),
                    new Tuple<double, string>(1002.34, "one thousand two dollars and 34 cents"),
                    new Tuple<double, string>(100000100001.34, "one hundred billion one hundred thousand one dollars and 34 cents"),
                    new Tuple<double, string>(123456789012.34, "one hundred twenty three billion four hundred fifty six million seven hundred eighty nine thousand twelve dollars and 34 cents")
                };

            foreach (var test in unitTests)
            {
                string result = MoneyToText(test.Item1);
                Debug.Assert(
                    test.Item2 == result,
                    string.Format(
                        "Test failed! Values are different->Expected: top, result: bottom.\n\r{0}\n\r{1}\n\r",
                        test.Item2,
                        result));
            }
        }

        public static string MoneyToText(double d)
        {
            if (d >= 200000000000.0 || d < 0.0)
            {
                throw new ArgumentOutOfRangeException("Number must be a positve value less than 200,000,000,000.00");
            }

            long n = Convert.ToInt64(Math.Truncate(d));
            int cents = Convert.ToInt32((d - n) * 100.0);
            return IntegerToText(n) + " dollars and " + cents.ToString("00") + " cents";
        }

        public static string IntegerToText(long n)
        {
            if (n >= 200000000000L || n < 0L)
            {
                throw new ArgumentOutOfRangeException("Number must be a positve value less than 200,000,000,000");
            }

            if (n < 20)
            {
                if (n > 9)
                {
                    return teens[n - 10];
                }

                return digits[n];
            }

            if (n < 100)
            {
                return tens[n / 10] + ((n % 10) != 0 ? " " + digits[n % 10] : string.Empty);
            }

            int i = 0;
            while (n < breaks[i])
            {
                i++;
            }

            string results= IntegerToText(n / breaks[i]) + " " + breakLabels[i] + " " + (n % breaks[i] != 0 ? IntegerToText(n % breaks[i]) : string.Empty);
            while (results.Contains("  "))
            {
                results = results.Replace("  ", " ");
            }

            return results;
        }
    }
}
