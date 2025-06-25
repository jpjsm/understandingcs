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
namespace WorkerRoleWithSBQueue1
{
    public class AzureStorageOperations
    {
        CloudStorageAccount StorageAccount { get; }
        CloudBlobClient BlobClient { get; }

        public AzureStorageOperations(string storageConnectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public void UploadBlob(string containerName, string blobName, Stream blobData)
        {
            CloudBlobContainer container = GetSetContainer(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadFromStream(blobData);
        }

        public void UploadBlob(string containerName, string blobName, string blobData)
        {
            CloudBlobContainer container = GetSetContainer(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadText(blobData);
        }

        private CloudBlobContainer GetSetContainer(string containerName)
        {
            // Retrieve a reference to a container.
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName.ToLowerInvariant());

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            return container;
        }
    }
}
