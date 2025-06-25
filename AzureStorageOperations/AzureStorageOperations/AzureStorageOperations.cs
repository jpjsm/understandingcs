using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;


namespace AzureStorageOperations
{
    public partial class AzureStorageOperations
    {
        private CloudStorageAccount StorageAccount { get; set; }
        public CloudBlobClient BlobClient { get; private set; }
        public CloudTableClient TableClient { get; private set; }

        public AzureStorageOperations(string storageConnectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
            TableClient = StorageAccount.CreateCloudTableClient();
        }

        public bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                BlobClient.GetRootContainerReference();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool TestConnection()
        {
            string errmsg;
            return TestConnection(out errmsg);
        }

    }
}
