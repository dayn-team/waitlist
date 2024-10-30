namespace Core.Application.Errors {
    public class LogicError : ApplicationException {
        public LogicError(string message = "Logic Error") : base(message) { }
    }
}
