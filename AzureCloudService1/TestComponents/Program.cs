using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestComponents
{
    using WorkerRoleWithSBQueue1;
    class Program
    {
        static void Main(string[] args)
        {
            AzureStorageOperations storage = new AzureStorageOperations(WorkerRole.StorageConnectionString);
            string blobName = string.Format("msg{0:D6}_{1}", 1, Guid.NewGuid());
            string blobData = "Very long body";
            storage.UploadBlob(WorkerRole.DefaultContainer, blobName, blobData);

            WorkerRole wr = new WorkerRole();

            Trace.WriteLine("Starting WorkerRole Logic");
            wr.OnStart();
            Thread t = new Thread(new ThreadStart(wr.Run));
            t.Start();
            //Task.Run(() => { wr.Run(); });

            Thread.Sleep(5000);
            wr.OnStop();

            Trace.Flush();
        }
    }
}
