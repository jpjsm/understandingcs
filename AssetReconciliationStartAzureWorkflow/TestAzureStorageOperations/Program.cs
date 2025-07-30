using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAzureStorageOperations
{
    using AssetReconciliationStartAzureWorkflow;
    class Program
    {
        static AzureStorageOperations client;
        static void Main(string[] args)
        {
            string cnxString = "DefaultEndpointsProtocol=https;AccountName=assetreconciliationstore;AccountKey=oDXZ1PIW/zDaiajhFYWs6frhk3ae7enItX1razln0qk0Oj3SSg6mbnfWzoBbn9D8FBSmwsseqK3QHtGag9KlvA==";
            client = new AzureStorageOperations(cnxString);
            //string errmsg;
            //client.DownloadBlob("inventory", "ServerFull.tsv", "\\tmp", out errmsg);

            //List<string> containersReferences = client.GetContainerNames().ToList();

            //foreach (var containerName in containersReferences)
            //{
            //    client.TraverseContainer(
            //        containerName,
            //        ContainerAction,
            //        DirsAction,
            //        BlobsAction,
            //        BlobsAction);
            //}

            client.TraverseContainers(
                    ContainerAction,
                    DirsAction,
                    BlobsAction,
                    BlobsAction);

        }

        public static void ContainerAction(AzureStorageContainerInfo container)
        {
            Console.WriteLine(
                "{0}{1}",
                new string(' ', container.Directory.Split('/').Length),
                container.Name);
        }

        public static void DirsAction(IEnumerable<AzureStorageContainerInfo> dirs)
        {
            foreach (var dir in dirs)
            {
                client.TraverseContainer(
                    dir,
                    ContainerAction,
                    DirsAction,
                    BlobsAction,
                    BlobsAction);
            }
        }

        public static void BlobsAction(IEnumerable<AzureBlobStorageReference> blobs)
        {
            foreach (var blob in blobs)
            {
                Console.WriteLine(
                    "{0}{1}, {2:N0} bytes",
                    new string(' ', blob.Directory.Split('/').Length),
                    blob.Name,
                    blob.Size);
            }
        }


        /*
        public static void TraverseContainer(AzureStorageOperations client, string containerName, int indentLevel, char indentChar = '\t')
        {
            if (indentLevel < 0)
            {
                indentLevel = 0;
            }

            AzureStorageContainerInfo item = client.GetContainerInfo(containerName);
            if (item==null)
            {
                return;
            }

            Console.WriteLine(
                "{0}{1}, {2}, {3}, {4}", 
                indentLevel > 0 ? new string(indentChar, indentLevel):string.Empty, 
                item.Name, 
                item.StorageType, 
                item.ContainerName, 
                item.Directory);
            if (item.Directories.Count > 0)
            {
                foreach (AzureStorageContainerInfo dir in item.Directories)
                {
                    TraverseDirectory(dir, indentLevel + 1, indentChar);
                }
            }

            if (item.BlockBlobs.Count > 0)
            {
                Console.WriteLine("{0}BlockBlobs:", indentLevel > 0 ? new string(indentChar, indentLevel) : string.Empty);
                foreach (var blob in item.BlockBlobs)
                {
                    Console.WriteLine(
                        "{0}{1}, {2}, {3}, {4}", 
                        indentLevel > 0 ? new string(indentChar, indentLevel+1) : indentChar.ToString(), 
                        blob.Name, 
                        blob.StorageType, 
                        blob.ContainerName, 
                        blob.Directory);
                }
            }

            if (item.PageBlobs.Count > 0)
            {
                Console.WriteLine("{0}PageBlobs:", indentLevel > 0 ? new string(indentChar, indentLevel) : string.Empty);
                foreach (var blob in item.PageBlobs)
                {
                    Console.WriteLine(
                        "{0}{1}, {2}, {3}, {4}", 
                        indentLevel > 0 ? new string(indentChar, indentLevel + 1) : indentChar.ToString(), 
                        blob.Name, 
                        blob.StorageType, 
                        blob.ContainerName, 
                        blob.Directory);
                }
            }
        }

        public static void TraverseDirectory(AzureStorageContainerInfo dir, int indentLevel, char indentChar = '\t')
        {
            Console.WriteLine(
                "{0}{1}, {2}, {3}, {4}",
                indentLevel > 0 ? new string(indentChar, indentLevel) : string.Empty,
                dir.Name,
                dir.StorageType,
                dir.ContainerName,
                dir.Directory);

            foreach (var subdir in dir.Directories)
            {
                TraverseDirectory(subdir, indentLevel + 1, indentChar);
            }

            if (dir.BlockBlobs.Count > 0)
            {
                Console.WriteLine(
                    "{0}BlockBlobs:",
                    indentLevel > 0 ? new string(indentChar, indentLevel) : string.Empty);
                foreach (var blob in dir.BlockBlobs)
                {
                    Console.WriteLine(
                        "{0}{1}, {2}, {3}, {4}",
                        indentLevel > 0 ? new string(indentChar, indentLevel + 1) : indentChar.ToString(),
                        blob.Name,
                        blob.StorageType,
                        blob.ContainerName,
                        blob.Directory);
                }
            }

            if (dir.PageBlobs.Count > 0)
            {
                Console.WriteLine("{0}PageBlobs:", indentLevel > 0 ? new string(indentChar, indentLevel) : string.Empty);
                foreach (var blob in dir.PageBlobs)
                {
                    Console.WriteLine(
                        "{0}{1}, {2}, {3}, {4}", indentLevel > 0 ? new string(indentChar, indentLevel + 1) : indentChar.ToString(),
                        blob.Name,
                        blob.StorageType,
                        blob.ContainerName,
                        blob.Directory);
                }
            }
        }

        public static void TraverseDirectory(AzureStorageOperations client, string containerName, string directoryPath, int indentLevel, char indentChar = '\t')
        {
            AzureStorageContainerInfo dir = client.GetDirectoryInfo(containerName, directoryPath);
            if (dir == null)
            {
                return;
            }

        }
        */
    }
}
