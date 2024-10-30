using Microsoft.AspNetCore.Http;

namespace Core.Application.Interfaces.Infrastructure.File {
    public interface IFileStorage {
        Task<string> uploadFileAsync(IFormFile file, string fileName);
        Task<bool> deleteFile(string fileName);
        Task<bool> getFileInto(string fileName, string nFile);
        string getSASToken(string filename, string fullFile, string contentType, long SASExpiryMins = -1);
    }
}
