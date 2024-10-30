namespace Core.Application.Errors {
    public class BadRequestError : Exception {
        public BadRequestError(string message = "Invalid Request") : base(message) { }
    }
}
