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
        public CloudBlobContainer GetSetContainer(string containerName)
        {
            // Retrieve a reference to a container.
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName.ToLowerInvariant());

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            return container;
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
    }
}
