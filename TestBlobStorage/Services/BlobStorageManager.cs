using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TestBlobStorage.Utilities;

namespace TestBlobStorage.Services
{
    public class BlobStorageManager : IStorageManager
    {
        public readonly BlobStorageOptions _storageOptions;

        public BlobStorageManager(IOptions<BlobStorageOptions> options)
        {
            _storageOptions = options.Value;
        }

        public bool DeleteFile(string fileName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
            BlobClient file = client.GetBlobClient(fileName);

            try
            {
                file.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
            BlobClient file = client.GetBlobClient(fileName);

            try
            {
                await file.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetSignedUrl(string fileName)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = contaionerClient.GetBlobClient(fileName);

            var signedUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.Now.AddHours(2)).AbsoluteUri;
            
            return signedUrl;
        }

        public async Task<string> GetSignedUrlAsync(string fileName)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var signedUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.UtcNow.AddHours(2)).AbsoluteUri;

            return await Task.FromResult(signedUrl);
        }

        public bool UploadFile(IFormFile formFile)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(formFile.FileName);

            try
            {
                blobClient.Upload(formFile.OpenReadStream(), new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = formFile.ContentType }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            try
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
