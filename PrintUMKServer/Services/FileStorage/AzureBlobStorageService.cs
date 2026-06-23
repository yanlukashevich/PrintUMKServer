using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace PrintUMKServer.Services.FileStorage
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString =
                configuration["AzureBlobStorage:ConnectionString"];

            var containerName =
                configuration["AzureBlobStorage:ContainerName"];

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);

            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file, string? userId, Guid jobId)
        {
            string blobPath = userId != null
                ? $"users/{userId}/{jobId}/{file.FileName}"
                : $"guests/{jobId}/{file.FileName}";

            var blobClient = _containerClient.GetBlobClient(blobPath);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobPath;
        }

        public string GenerateReadSasUrl(string blobPath, TimeSpan validFor)
        {
            var blobClient = _containerClient.GetBlobClient(blobPath);

            if (!blobClient.CanGenerateSasUri)
                throw new InvalidOperationException("BlobClient cannot generate SAS");

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }

    }
}
