using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VcClient;
using VcClientExceptions;

namespace UnderstandingCosmos
{
    class Program
    {
        static void Main(string[] args)
        {
            string vc = "vc://cosmos11/HWInventory";

            VC.Setup(vc, VC.NoProxy, null);

            // Test data
            string testdata = "/my/matchedServerRackSummary.ss";

            // destination tsv
            string destinationTextFile = "matchedServerRackSummary.tsv";

            // VC.Download(testdata, destinationTextFile, compression: true, overwrite: true);

            //using (FileStream outputFile = new FileStream(destinationTextFile, FileMode.OpenOrCreate, FileAccess.Write))
            //{
            //    VC.ReadStream(testdata, compression: true).CopyTo(outputFile);
            //}
            
            
        }
    }
}
