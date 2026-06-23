namespace PrintUMKServer.Services.FileStorage
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile File, string? userId, Guid jobId);

        string GenerateReadSasUrl(string blobPath, TimeSpan validFor);
    }
}
