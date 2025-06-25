using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingDateTimeInitialization
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime somewhereintime = DateTime.MinValue;
            Console.WriteLine("Somewhere in time is: {0}", somewhereintime.ToString("yyyy-MM-dd HH:mm:ss.fffff"));
        }
    }
}
