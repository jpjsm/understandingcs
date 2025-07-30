using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingGuid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Guid without dashes: {0}", Guid.NewGuid().ToString("N"));
        }
    }
}
