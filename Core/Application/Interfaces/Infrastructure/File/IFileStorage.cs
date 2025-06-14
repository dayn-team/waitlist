using Microsoft.AspNetCore.Http;

namespace Core.Application.Interfaces.Infrastructure.File {
    public interface IFileStorage {
        Task<string> UploadFileAsync(IFormFile file, string fileName);
        Task<bool> DeleteFile(string fileName);
        Task<bool> GetFileInto(string fileName, string nFile);
        string GetSASToken(string filename, string fullFile, string contentType, long SASExpiryMins = -1);
    }
}
