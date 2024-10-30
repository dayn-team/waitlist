using Infrastructure.Abstraction.HTTP;
using Infrastructure.DTO;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Integration.HTTP {
    [RegisterAsSingleton]
    public class HttpWebRequestClient : IHttpClient {
        private HttpClient _client;
        public HttpWebRequestClient() {
            _client = new HttpClient();
        }
        public async Task<HttpClientResponseMessage> sendRequest(string url, HttpMethod method, HttpContent? content, Dictionary<string, string> headers) {
            HttpClientResponseMessage response = new HttpClientResponseMessage();
            try {
                var res = await requestHandler(url, method, content, headers);
                response.httpStatus = (int)res.StatusCode;
                response.successful = false;
                if (res.IsSuccessStatusCode) {
                    response.successful = true;
                    response.result = await res.Content.ReadAsStringAsync();
                } else {
                    response.error = await res.Content.ReadAsStringAsync();
                }
            } catch (Exception err) {
                response.successful = false;
                response.httpStatus = -1;
                response.exception = err;
            }
            return response;
        }

        private async Task<HttpResponseMessage> requestHandler(string url, HttpMethod method, HttpContent? content, Dictionary<string, string> headers) {
            using (var client = new HttpClient()) {
                var req = new HttpRequestMessage(method, url);
                if (content != null) {
                    req.Content = content;
                }
                if (headers != null) {
                    foreach (KeyValuePair<string, string> header in headers) {
                        req.Headers.Add(header.Key, header.Value);
                    }
                }
                var res = await client.SendAsync(req);
                return res;
            }
        }

        public async Task downloadFile(string url, string path) {
            using (var client = new HttpClient()) {
                client.Timeout = TimeSpan.FromMinutes(5);
                // Create a file stream to store the downloaded data.
                // This really can be any type of writeable stream.
                using (var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
                    // Use the custom extension method below to download the data.
                    // The passed progress-instance will receive the download status updates.
                    await downloadAsync(client, url, file);
                }
            }
        }

        private async Task downloadAsync(HttpClient client, string requestUri, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default) {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead)) {
                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync()) {
                    // Ignore progress reporting when no progress reporter was 
                    // passed or when the content length is unknown
                    if (progress == null || !contentLength.HasValue) {
                        await download.CopyToAsync(destination);
                        return;
                    }

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
                    // Use extension method to report progress while downloading
                    await CopyToAsync(download, destination, 81920, relativeProgress, cancellationToken);
                    progress.Report(1);
                }
            }
        }

        private async Task CopyToAsync(Stream source, Stream destination, int bufferSize, IProgress<long>? progress = null, CancellationToken cancellationToken = default) {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0) {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}
