using System.Collections.Generic;

namespace AzureStorageOperations
{
    public class AzureStorageContainerInfo : AzureBlobStorageReference
    {
        public IList<AzureStorageContainerInfo> Directories { get; set; }
        public IList<AzureBlobStorageReference> BlockBlobs { get; set; }
        public IList<AzureBlobStorageReference> PageBlobs { get; set; }
    }
}