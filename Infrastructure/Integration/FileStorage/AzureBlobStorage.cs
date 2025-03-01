using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Application.Interfaces.Infrastructure.File;
using Core.Domain.DTOs.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Integration.FileStorage {
    [RegisterAsScoped]
    public class AzureBlobStorage : IFileStorage {
        private readonly SystemVariables? _sysVar;
        private readonly AzureStorage _azureStore;
        private BlobServiceClient _blobServiceClient;
        public AzureBlobStorage(IOptionsMonitor<SystemVariables> config) {
            _sysVar = config.CurrentValue;
            _azureStore = _sysVar.AzureStorage;
            _blobServiceClient = new BlobServiceClient(_azureStore.connectionString);
        }
        public AzureBlobStorage(SystemVariables config) {
            _sysVar = config;
            _azureStore = _sysVar.AzureStorage;
            _blobServiceClient = new BlobServiceClient(_azureStore.connectionString);
        }
        public async Task<bool> deleteFile(string fileName) {
            try {
                var container = await createContainerAsync(_azureStore.containerName);
                BlobClient blobClient = container.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
                return true;
            } catch (Exception ex) {
                // Let the user know that the directory does not exist
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> getFileInto(string fileName, string nFile) {
            try {
                var container = await createContainerAsync(_azureStore.containerName);
                BlobClient blobClient = container.GetBlobClient(fileName);
                await blobClient.DownloadToAsync(nFile);
                return true;
            } catch (Exception ex) {
                // Let the user know that the directory does not exist
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            return false;
        }

        public async Task<FileStream?> getFileIntoStream(string fileName, string streamFolder) {
            try {
                if (!Directory.Exists(streamFolder)) {
                    Directory.CreateDirectory(streamFolder);
                }
                var container = await createContainerAsync(_azureStore.containerName);
                FileStream fileStream = File.OpenWrite(streamFolder);
                BlobClient blobClient = container.GetBlobClient(fileName);
                await blobClient.DownloadToAsync(fileStream);
                return fileStream;
            } catch (Exception ex) {
                // Let the user know that the directory does not exist
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            return null;
        }

        public async Task<string> uploadFileAsync(IFormFile file, string fileName) {
            var container = await createContainerAsync(_azureStore.containerName);
            BlobClient blobClient = container.GetBlobClient(fileName);
            using (var memoryStream = new MemoryStream()) {
                await file.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var dataObject = await blobClient.UploadAsync(memoryStream, true);
                await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = file.ContentType });
                return blobClient.Uri.AbsoluteUri.ToString();
            }
        }

        private async Task<BlobContainerClient> createContainerAsync(string containerName) {
            BlobContainerClient container;
            var containerRef = _blobServiceClient.GetBlobContainerClient(containerName);
            if (containerRef.Exists()) {
                return containerRef;
            }
            container = await _blobServiceClient.CreateBlobContainerAsync(containerName);
            return container;
        }

        public string getSASToken(string filename, string fullFile, string contentType, long SASExpiryMins = -1) {
            SASExpiryMins = SASExpiryMins > 0 ? SASExpiryMins : _azureStore.SASExpiryMins;
            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder() {
                BlobContainerName = _azureStore.containerName,
                BlobName = filename,
                ExpiresOn = DateTime.Now.AddMinutes(SASExpiryMins),
                ContentType = contentType,
                StartsOn = DateTime.Now.AddMinutes(-15)
            };
            blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);
            var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_azureStore.accountName, _azureStore.accountKey)).ToString();
            var sasUrl = fullFile + "?" + sasToken;
            return sasUrl;
        }
    }
}
