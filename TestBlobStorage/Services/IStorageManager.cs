namespace TestBlobStorage.Services
{
    public interface IStorageManager
    {
        string GetSignedUrl(string fileName);
        Task<string> GetSignedUrlAsync(string fileName);
        bool UploadFile(IFormFile formFile);
        Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType);

        bool DeleteFile(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
    }
}
