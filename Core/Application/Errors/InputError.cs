namespace Core.Application.Errors {
    public class InputError : ApplicationException {
        public InputError(string message = "Invalid input.") : base(message) { }
    }
}
