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
using Microsoft.WindowsAzure.Storage.Shared.Protocol;


namespace AssetReconciliationStartAzureWorkflow
{
    public enum AzureBlobStorageTypes
    {
        Unknown,
        Container,
        BlockBlob,
        PageBlob,
        AppendBlob,
        Directory
    }

    public class AzureBlobStorageReference
    {
        public string Name { get; set; }
        public string ContainerName { get; set; }
        public string Directory { get; set; }
        public long  Size { get; set; }
        public string BlobName { get; set; }
        public AzureBlobStorageTypes StorageType { get; set; }
    }

    public class AzureStorageContainerInfo : AzureBlobStorageReference
    {
        public IList<AzureStorageContainerInfo> Directories { get; set; }
        public IList<AzureBlobStorageReference> BlockBlobs { get; set; }
        public IList<AzureBlobStorageReference> PageBlobs { get; set; }
    }

    public class AzureStorageOperations
    {
        public CloudStorageAccount StorageAccount { get; }
        public CloudBlobClient BlobClient { get; }

        public AzureStorageOperations(string storageConnectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
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

        public IEnumerable<string> GetContainerNames()
        {
            return BlobClient.ListContainers().Select(c => c.Name);
        }

        public IEnumerable<AzureStorageContainerInfo> GetContainersReferences()
        {
            return BlobClient.ListContainers()
                .Select(c => GetContainerInfo(c.Name));
        }

        public AzureStorageContainerInfo GetContainerInfo(string containerName)
        {
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);

            try
            {
                if (!container.Exists())
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            AzureStorageContainerInfo containerInfo = new AzureStorageContainerInfo
            {
                Name = containerName,
                ContainerName = containerName,
                Directory = string.Empty,
                StorageType = AzureBlobStorageTypes.Container,
                Directories = new List<AzureStorageContainerInfo>(),
                BlockBlobs = new List<AzureBlobStorageReference>(),
                PageBlobs = new List<AzureBlobStorageReference>()
            };

            foreach (IListBlobItem item in container.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = item as CloudBlobDirectory;

                    containerInfo.Directories.Add(
                        GetDirectoryInfo(
                            containerName,
                            dir.Prefix));

                    continue;
                }

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    InsertCloudBlockBlobToContainerInfo(containerName, containerInfo, item);

                    continue;
                }

                if (item.GetType() == typeof(CloudPageBlob))
                {
                    InsertCloudPageBlobToContainerInfo(containerName, containerInfo, item);

                    continue;
                }
            }

            return containerInfo;
        }


        public AzureStorageContainerInfo GetDirectoryInfo(string containerName, string directoryPath)
        {
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
            CloudBlobDirectory dir;

            try
            {
                if (!container.Exists())
                {
                    return null;
                }

                dir = container.GetDirectoryReference(directoryPath);
            }
            catch (Exception)
            {
                return null;
            }


            AzureStorageContainerInfo containerInfo = new AzureStorageContainerInfo
            {
                Name = Path.GetFileName(
                    directoryPath.LastIndexOf('/') != (directoryPath.Length - 1) ?
                    directoryPath :
                    directoryPath.Substring(0, (directoryPath.Length - 1))),
                ContainerName = containerName,
                Directory = directoryPath,
                StorageType = AzureBlobStorageTypes.Directory,
                Directories = new List<AzureStorageContainerInfo>(),
                BlockBlobs = new List<AzureBlobStorageReference>(),
                PageBlobs = new List<AzureBlobStorageReference>()
            };

            foreach (IListBlobItem item in dir.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory subdir = item as CloudBlobDirectory;
                    containerInfo.Directories.Add(
                        GetDirectoryInfo(
                            containerName,
                            subdir.Prefix));

                    continue;
                }

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    InsertCloudBlockBlobToContainerInfo(containerName, containerInfo, item);

                    continue;
                }

                if (item.GetType() == typeof(CloudPageBlob))
                {
                    InsertCloudPageBlobToContainerInfo(containerName, containerInfo, item);

                    continue;
                }
            }

            return containerInfo;
        }

        public void TraverseContainers(
            Action<AzureStorageContainerInfo> containerAction,
            Action<IEnumerable<AzureStorageContainerInfo>> dirs, 
            Action<IEnumerable<AzureBlobStorageReference>> blockBlobs,
            Action<IEnumerable<AzureBlobStorageReference>> pageBlobs
            )
        {
            foreach (var containerName in GetContainerNames())
            {
                TraverseContainer(containerName, containerAction, dirs, blockBlobs, pageBlobs);
            }
        }

        public void TraverseContainer(
            string containerName,
            Action<AzureStorageContainerInfo> containerAction,
            Action<IEnumerable<AzureStorageContainerInfo>> dirs,
            Action<IEnumerable<AzureBlobStorageReference>> blockBlobs,
            Action<IEnumerable<AzureBlobStorageReference>> pageBlobs
            )
        {
            AzureStorageContainerInfo item = GetContainerInfo(containerName);
            if (item == null)
            {
                return;
            }

            containerAction(item);

            if (item.Directories.Count > 0)
            {
                dirs(item.Directories);
            }

            if (item.BlockBlobs.Count > 0)
            {
                blockBlobs(item.BlockBlobs);
            }

            if (item.PageBlobs.Count > 0)
            {
                blockBlobs(item.PageBlobs);
            }
        }

        public void TraverseContainer(
            AzureStorageContainerInfo item,
            Action<AzureStorageContainerInfo> containerAction,
            Action<IEnumerable<AzureStorageContainerInfo>> dirs,
            Action<IEnumerable<AzureBlobStorageReference>> blockBlobs,
            Action<IEnumerable<AzureBlobStorageReference>> pageBlobs
            )
        {
            if (item == null)
            {
                return;
            }

            containerAction(item);

            if (item.Directories.Count > 0)
            {
                dirs(item.Directories);
            }

            if (item.BlockBlobs.Count > 0)
            {
                blockBlobs(item.BlockBlobs);
            }

            if (item.PageBlobs.Count > 0)
            {
                blockBlobs(item.PageBlobs);
            }
        }

        private static void InsertCloudBlockBlobToContainerInfo(string containerName, AzureStorageContainerInfo containerInfo, IListBlobItem item)
        {
            CloudBlockBlob blob = item as CloudBlockBlob;
            blob.SetProperties();
            int start = blob.Uri.AbsoluteUri.IndexOf(containerName) - 1;
            int length = blob.Uri.AbsoluteUri.Length - (blob.Name.Length + start + 1);
            containerInfo.BlockBlobs.Add(
                new AzureBlobStorageReference()
                {
                    Name = Path.GetFileName(blob.Name),
                    ContainerName = containerName,
                    Directory = blob.Uri.AbsoluteUri.Substring(start, length),
                    StorageType = AzureBlobStorageTypes.BlockBlob,
                    Size = blob.Properties.Length,
                    BlobName = blob.Name
                });
        }

        private static void InsertCloudPageBlobToContainerInfo(string containerName, AzureStorageContainerInfo containerInfo, IListBlobItem item)
        {
            CloudPageBlob blob = item as CloudPageBlob;
            blob.SetProperties();
            int start = blob.Uri.AbsoluteUri.IndexOf(containerName) - 1;
            int length = blob.Uri.AbsoluteUri.Length - (blob.Name.Length + start + 1);
            containerInfo.PageBlobs.Add(
                new AzureBlobStorageReference()
                {
                    Name = Path.GetFileName(blob.Name),
                    ContainerName = containerName,
                    Directory = blob.Uri.AbsoluteUri.Substring(start, length),
                    StorageType = AzureBlobStorageTypes.BlockBlob,
                    Size = blob.Properties.Length,
                    BlobName = blob.Name
                });
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
