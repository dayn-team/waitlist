namespace Core.Application.Errors {
    public class NotFoundError : Exception {
        public NotFoundError(string message = "Resource not found"):base(message) {

        }
    }
}
