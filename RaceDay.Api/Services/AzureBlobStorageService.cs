using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace RaceDay.Api.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly StorageSharedKeyCredential _credential;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration["AzureStorage:ConnectionString"]
                ?? throw new InvalidOperationException(
                    "Azure Storage connection string is missing.");

            string containerName =
                configuration["AzureStorage:ContainerName"]
                ?? "raceday-files";

            var settings = connectionString.Split(';')
                .Select(part => part.Split('=', 2))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.OrdinalIgnoreCase);

            string accountName = settings["AccountName"];
            string accountKey = settings["AccountKey"];

            _credential = new StorageSharedKeyCredential(
                accountName, accountKey);

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadFileAsync(
            IFormFile file, string eventId)
        {
            await _containerClient.CreateIfNotExistsAsync(
                PublicAccessType.None);

            string extension = Path.GetExtension(file.FileName);

            string blobName =
                $"events/{eventId}/{Guid.NewGuid()}{extension}";

            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    }
                });

            return blobClient.Uri.ToString();
        }

        public string GetReadUrl(string blobUrl)
        {
            var blobClient = new BlobClient(
                new Uri(blobUrl), _credential);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(12)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }

    }
}
