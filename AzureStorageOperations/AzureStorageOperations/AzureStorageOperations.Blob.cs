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
        public void UploadBlob(string containerName, string blobName, Stream blobData)
        {
            CloudBlobContainer container = GetSetContainer(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadFromStream(blobData);
        }

        public void UploadStringBlob(string containerName, string blobName, string blobData)
        {
            CloudBlobContainer container = GetSetContainer(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadText(blobData);
        }

        public void DownloadBlobToFile(string containerName, string blobName, string destinationFolder, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(destinationFolder) && !Directory.Exists(destinationFolder))
            {
                errorMessage = string.Format("[ERROR - NOT FOUND] Destination folder: {0}, doesn't exist.", destinationFolder);
            }

            try
            {
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
                if (container.Exists())
                {
                    CloudBlockBlob blobSource = container.GetBlockBlobReference(blobName);
                    if (blobSource.Exists())
                    {
                        blobSource.DownloadToFile(Path.Combine(destinationFolder.Trim(), blobName), FileMode.Create);
                    }
                    else
                    {
                        errorMessage = string.Format("[ERROR - NOT FOUND] Blob: {0}, doesn't exist.", blobName);
                    }
                }
                else
                {
                    errorMessage = string.Format("[ERROR - NOT FOUND] Container: {0}, doesn't exist.", containerName);
                }

            }
            catch (Exception ex)
            {
                errorMessage = string.Format("[ERROR - EXCEPTION THROWN] exception message: {0}.", ex.Message);
            }

        }

        public string DownloadBlobToString(string containerName, string blobName, out string errorMessage)
        {
            errorMessage = string.Empty;

            string results = null;
            try
            {
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
                if (container.Exists())
                {
                    CloudBlockBlob blobSource = container.GetBlockBlobReference(blobName);
                    if (blobSource.Exists())
                    {
                        results = blobSource.DownloadText() ?? string.Empty;
                    }
                    else
                    {
                        errorMessage = string.Format("[ERROR - NOT FOUND] Blob: {0}, doesn't exist.", blobName);
                    }
                }
                else
                {
                    errorMessage = string.Format("[ERROR - NOT FOUND] Container: {0}, doesn't exist.", containerName);
                }

            }
            catch (Exception ex)
            {
                errorMessage = string.Format("[ERROR - EXCEPTION THROWN] exception message: {0}.", ex.Message);
            }

            return results;
        }

        public IEnumerable<string> GetBlobsFlatList(string containerName, out string errorMessage)
        {
            errorMessage = string.Empty;
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
            if (container == null || !container.Exists())
            {
                errorMessage = string.Format("Container: {0} doesn't exist.", containerName);
            }

            return container.ListBlobs(useFlatBlobListing: true).Select(b => (b as CloudBlob).Name);
        }

    }
}
