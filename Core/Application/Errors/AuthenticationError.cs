namespace Core.Application.Errors {
    public class AuthenticationError : ApplicationException { 
        public AuthenticationError(string message = "Invalid Profile. Access denied") : base(message) { }
    }
}
