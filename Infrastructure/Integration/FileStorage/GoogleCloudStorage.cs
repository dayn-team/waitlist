using Core.Application.Interfaces.Infrastructure.File;
using Core.Domain.DTOs.Configurations;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Libraries.FileStorage {
    [DoNotAutoRegister]
    public class GoogleCloudStorage : IFileStorage {
        private readonly SystemVariables? _sysVar;
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly string bucketName;
        public GoogleCloudStorage(IOptionsMonitor<SystemVariables> config) {
            this._sysVar = config.CurrentValue;
            googleCredential = GoogleCredential.FromFile(_sysVar.GoogleCloudStorageConfig.credentialFile);
            storageClient = StorageClient.Create(googleCredential);
            this.bucketName = _sysVar.GoogleCloudStorageConfig.bucketName;
        }

        public GoogleCloudStorage(string keyFile, string bucketName) {
            this._sysVar = null;
            googleCredential = GoogleCredential.FromFile(keyFile);
            storageClient = StorageClient.Create(googleCredential);
            this.bucketName = bucketName;
        }

        public async Task<bool> deleteFile(string fileName) {
            await storageClient.DeleteObjectAsync(bucketName, fileName);
            return true;
        }

        public async Task<string> uploadFileAsync(IFormFile file, string fileName) {
            using (var memoryStream = new MemoryStream()) {
                await file.CopyToAsync(memoryStream);
                var dataObject = await storageClient.UploadObjectAsync(bucketName, fileName, null, memoryStream);
                return dataObject.MediaLink;
            }
        }

        public async Task<bool> getFileInto(string fileName, string nFile) {
            using var outputFile = File.OpenWrite(nFile);
            await storageClient.DownloadObjectAsync(bucketName, fileName, outputFile);
            return true;
        }

        public string getSASToken(string filename, string fullURI, string contentType) {
            throw new NotImplementedException();
        }

        public string getSASToken(string filename, string fullFile, string contentType, long SASExpiryMins = -1) {
            throw new NotImplementedException();
        }
    }
}
