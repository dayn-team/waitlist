namespace Core.Application.Interfaces.Infrastructure.Cache {
    public interface ICacheService {
        Task<T?> GetWithKey<T>(string key);
        Task<string?> GetWithKey(string key);
        Task<bool> AddWithKey(string key, string value, int expiry = 600, int slidingExp=600);
        Task<bool> AddWithKey<T>(string key, T value, int expiry = 600, int slidingExp=600);
        Task<bool> DeleteWithKey(string key);
    }
}
