namespace AzureStorageOperations
{
    public class AzureBlobStorageReference
    {
        public string Name { get; set; }
        public string ContainerName { get; set; }
        public string Directory { get; set; }
        public long Size { get; set; }
        public string BlobName { get; set; }
        public AzureBlobStorageTypes StorageType { get; set; }
    }
}