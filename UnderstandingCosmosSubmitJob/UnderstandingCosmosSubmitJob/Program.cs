using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VcClient;


namespace UnderstandingCosmosSubmitJob
{
    class Program
    {
        public const bool UseCompression = true;
        public const bool OverwriteIfExists = true;
        public const uint DefaultWaitTimeSeconds = 10;
        static void Main(string[] args)
        {

            // Setup Cosmos environment
            string vc = "vc://cosmos11/HWInventory";
            VC.Setup(vc, VC.NoProxy, null);

            // Job script
            string script = "structuredStream = SSTREAM @@inputStream@@; OUTPUT structuredStream TO @@outputStream@@;";

            // Save script to file
            string scopeScriptFileName = "ConvertSStoTSV.script";
            File.WriteAllText(scopeScriptFileName, script);

            // Build parameter dictionary; remember to add enclosing quotes to every parameter
            Dictionary<string, string> jobParams = new Dictionary<string, string>();
            jobParams.Add("inputStream", "\"/local/Inventory/Rack.Clean.Inventory.ss\"");
            jobParams.Add("outputStream", "\"/local/test/fromSStoTSV/MatchedForUpdates.tsv\"");

            ScopeClient.SubmitParameters submitParams = new ScopeClient.SubmitParameters(scopeScriptFileName);
            submitParams.FriendlyName = "ConvertSStoTSV." + DateTime.UtcNow.ToString("yyyy-MM-ddTHHmmss");
            submitParams.Parameters = jobParams;
            JobInfo jobinfo = ScopeClient.Scope.Submit(submitParams);

            TimeSpan waitTime = new TimeSpan(TimeSpan.TicksPerSecond * (int)DefaultWaitTimeSeconds);
            while (true)
            {
                jobinfo = VcClient.VC.GetJobInfo(jobinfo.ID, UseCompression);
                Console.WriteLine("Job State = {0}", jobinfo.State);
                if (jobinfo.State == VcClient.JobInfo.JobState.Cancelled || jobinfo.State == VcClient.JobInfo.JobState.Completed
                    || jobinfo.State == VcClient.JobInfo.JobState.CompletedFailure
                    || jobinfo.State == VcClient.JobInfo.JobState.CompletedSuccess)
                {
                    Console.WriteLine("Job Stopped Running");
                    break;
                }

                System.Threading.Thread.Sleep(waitTime);
            }
        }

    }
}
