using Infrastructure.Model;

namespace Infrastructure.Abstraction.HTTP {
    public interface IHttpClient {
        Task<HttpClientResponseMessage> sendRequest(string url, HttpMethod method, HttpContent? content, Dictionary<string, string> headers);
    }
}
