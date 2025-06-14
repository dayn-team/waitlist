namespace Core.Application.Interfaces.Infrastructure.Identity {
    public interface IIdentityManager {
        string Message { get; }
        string Useragent { get; }
        bool Valid { get; }
        bool SessionValid();
        string GetJWTIdentity(Dictionary<string, string> identity, int expiry = 0);
        T GetProfile<T>();
        string GetHeaderValue(string key);
        IDictionary<string, object> GetAllHeader();
        void LoadCustomHeaders(IDictionary<string, object> header);
        string IPAddress { get; }
        string EndPointAddress { get; }
    }
}
