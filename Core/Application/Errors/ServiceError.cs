namespace Core.Application.Errors {
    public class ServiceError : Exception {
        public ServiceError(string message = "Service Error") : base(message) { }
    }
}
