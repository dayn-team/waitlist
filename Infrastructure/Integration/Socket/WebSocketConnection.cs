using Core.Shared;
using Infrastructure.Abstraction.Socket;
using Infrastructure.Model;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Timers;

namespace Infrastructure.Integration.Socket {
    public abstract class WebSocketConnection : IDisposable {
        protected readonly WebSocket _webSocket;
        public Encoding Encoding { get; }
        protected readonly IWebSocketManager _socketManager;
        protected long lastActivity = 0;
        protected long connectionTime = 0;
        private System.Timers.Timer timer = new System.Timers.Timer();
        public string connectionID { get; private set; }
        private readonly AsyncQueue<SocketData> _sendQueue = new AsyncQueue<SocketData>();
        private Task? _communicationTask;
        public WebSocketConnection(WebSocket webSocket, IWebSocketManager socketManager, Encoding? encoding = null) {
            _webSocket = webSocket;
            Encoding = encoding ?? Encoding.UTF8;
            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(liveNessCheck);
            lastActivity = Utilities.getTodayDate().unixTimestamp;
            connectionTime = Utilities.getTodayDate().unixTimestamp;
            connectionID = Cryptography.CharGenerator.genID();
            _socketManager = socketManager;
            timer.Start();
        }

        protected virtual async void liveNessCheck(object? sender, ElapsedEventArgs e) {
            //checks every 3 secs for liveness
            long dormantTime = Utilities.getTodayDate().unixTimestamp - lastActivity;
            if (dormantTime >= 3000) {
                if (_webSocket.State != WebSocketState.Open) {
                    await closeSocketAsync();
                }
            }
            if (dormantTime > 60000)
                await closeSocketAsync();
        }

        public Task handleCommunicationAsync() {
            if (_communicationTask == null) {
                _communicationTask = Task.WhenAll(receiveTask(), sendTask());
            }
            return _communicationTask;
        }

        public void consumeMessage(string data) {
            _sendQueue.enqueue(new SocketData { data = data });
        }

        public void consumeMessage(object data) {
            _sendQueue.enqueue(new SocketData { data = JObject.FromObject(data).ToString() });
        }

        public Task closeAsync() {
            if (_communicationTask == null) return closeSocketAsync();
            _sendQueue.enqueue(new SocketData { closeRequest = true });
            return _communicationTask;
        }

        protected Task closeSocketAsync() {
            return _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
        }

        private async Task receiveTask() {
            var messageSize = new byte[1024 * 10];
            var buffer = new ArraySegment<byte>(messageSize);
            while (true) {
                try {
                    var message = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    var messageObj = Encoding.Default.GetString(new ArraySegment<byte>(messageSize, 0, message.Count).ToArray());
                    await processRequest(messageObj);
                } catch (Exception ex) {
                    bool canIgnoreException = (ex as WebSocketException)?.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely;
                    if (!canIgnoreException) {
                        Debug.WriteLine(ex);
                    }
                }
                if (_webSocket.State != WebSocketState.Open) {
                    _sendQueue.enqueueIfEmpty(new SocketData { closeRequest = true });
                    return;
                }
            }
        }

        private async Task sendTask() {
            while (true) {
                SocketData[] toSend = await _sendQueue.dequeueAsync();
                foreach (var request in toSend) {
                    if (request.closeRequest) {
                        if (_webSocket.State == WebSocketState.Open) {
                            await closeSocketAsync();
                        }
                        return;
                    }
                    if (_webSocket.State != WebSocketState.Open) {
                        throw new InvalidOperationException("Write operation failed, the socket is no longer open");
                    }
                    var buffer = Encoding.UTF8.GetBytes(request.data);
                    var sendTask = _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    await sendTask.ConfigureAwait(false);
                }
            }
        }

        protected abstract Task processRequest(string message);

        public virtual void Dispose() {
            try {
                _ = closeSocketAsync();
            } catch { }
        }
    }
}
