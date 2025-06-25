using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MocaDataProcess.Test
{
    using Microsoft.Mcio.Reconciliation;

    class MocaDataProcessTest
    {
        static int Main(string[] args)
        {
            MocaDataProcess moca = new MocaDataProcess();

            if (moca.OnStart())
            {
                moca.Run();
                Console.Write("Press any key to terminate test.");
                var keyinfo = Console.ReadKey();
                moca.OnStop();
            }
            else
            {
                Console.Write("ERROR: Failed to start.");
                return -1;
            }

            Console.WriteLine("Execution completed.");
            return 0;
        }
    }
}
