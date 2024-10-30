using Infrastructure.Integration.Socket;

namespace Infrastructure.Abstraction.Socket {
    public interface IWebSocketManager {
        void addConnection(string clientID, WebSocketConnection connection);
        void removeConnection(string clientID, WebSocketConnection connection);
        void sendMessage(string message);
        void sendMessage(string message, string clientID);
        Task closeAllConnections();
        Task closeConnection(string clientID);
        Task closeConnection(string clientID, WebSocketConnection connection);
    }
}
