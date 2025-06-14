namespace Core.Application.Errors {
    public class AuthorizationError : ApplicationException {
        public AuthorizationError(string message = "Invalid Profile. Access denied") : base(message) { }
    }
}
