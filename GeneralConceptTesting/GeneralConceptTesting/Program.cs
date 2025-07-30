using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralConceptTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime utcDatetime = DateTime.UtcNow;

            Console.WriteLine(utcDatetime.Kind);

            string[] nullStrings = new string[10];

            string concatenatedNullStrings = string.Join("\t", nullStrings);
        }
    }
}
