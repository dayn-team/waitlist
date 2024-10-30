namespace Infrastructure.Model {
    public class SocketDataBase {
        public bool closeConnection { get; set; } = false;
    }
    public class SocketData {
        public string data;
        public bool closeRequest;
    }
}
