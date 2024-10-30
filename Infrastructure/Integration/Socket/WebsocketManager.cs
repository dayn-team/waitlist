using Infrastructure.Abstraction.Socket;
using NetCore.AutoRegisterDi;
using System.Collections.Concurrent;

namespace Infrastructure.Integration.Socket {
    [RegisterAsSingleton]
    public class WebsocketManager : IWebSocketManager, IDisposable {
        private ConcurrentDictionary<string, List<WebSocketConnection>> _sockets = new ConcurrentDictionary<string, List<WebSocketConnection>>();
        public void addConnection(string clientID, WebSocketConnection connection) {
            if (string.IsNullOrEmpty(clientID))
                throw new Exception("Client ID is required");
            _sockets.AddOrUpdate(clientID, new List<WebSocketConnection> { connection }, (a, b) => { b.Add(connection); return b; });
        }

        public async Task closeAllConnections() {
            foreach (KeyValuePair<string, List<WebSocketConnection>> data in _sockets) {
                for (int i = 0; i < data.Value.Count; i++) {
                    await data.Value[i].closeAsync();
                }
            }
            _sockets.Clear();
            return;
        }

        public async Task closeConnection(string clientID) {
            var data = _sockets[clientID];
            for (int i = 0; i < data.Count; i++) {
                await data[i].closeAsync();
            }
            var g = new List<WebSocketConnection>();
            _sockets.Remove(clientID, out g);
        }

        public async Task closeConnection(string clientID, WebSocketConnection connection) {
            var data = _sockets[clientID];
            var connectionObj = data.Find(B => B == connection);
            if (connectionObj != null)
                await connectionObj.closeAsync();
            removeConnection(clientID, connection);
        }

        public void Dispose() {
            try {
                _ = closeAllConnections();
            } catch { }
        }

        public void removeConnection(string clientID, WebSocketConnection connection) {
            _sockets.AddOrUpdate(clientID, new List<WebSocketConnection> { connection }, (a, b) => {
                b.Remove(connection); return b;
            });
        }

        public void sendMessage(string message) {
            foreach (KeyValuePair<string, List<WebSocketConnection>> data in _sockets) {
                for (int i = 0; i < data.Value.Count; i++) {
                    data.Value[i].consumeMessage(message);
                }
            }
        }

        public void sendMessage(string message, string clientID) {
            var data = _sockets[clientID];
            for (int i = 0; i < data.Count; i++) {
                data[i].consumeMessage(message);
            }
        }
    }
}
