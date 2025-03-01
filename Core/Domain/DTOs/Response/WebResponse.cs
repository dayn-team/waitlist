using Core.Application.Errors;

namespace Core.Domain.DTOs.Response {
    public class WebResponse<T> {
        public StatusIdentifier Result { get; private set; }
        public T? Content { get; private set; }
        public int statusCode { get; private set; } = 200;
        public WebResponse() {

        }
        public WebResponse<T> fail(ResponseCodes status, string? details = null) {
            if ((int)status == 0x00)
                throw new LogicError();
            this.Result = new StatusIdentifier { status = (int)status, message = status.ToString().Replace("_", " "), details = details };
            this.Content = default(T);
            getStatusCode(status);
            return this;
        }
        private void getStatusCode(ResponseCodes status) {
            switch (status) {
                case ResponseCodes.ACCESS_DENIED_ERROR:
                    this.statusCode = 401;
                    break;
                case ResponseCodes.RESOURCE_NOT_FOUND:
                    this.statusCode = 404;
                    break;
                case ResponseCodes.GENERIC_ERROR:
                    this.statusCode = 400;
                    break;
                case ResponseCodes.INPUT_ERROR:
                    this.statusCode = 400;
                    break;
                case ResponseCodes.INVALID_REQUEST:
                    this.statusCode = 403;
                    break;
                case ResponseCodes.LOGIC_ERROR:
                    this.statusCode = 422;
                    break;
                case ResponseCodes.SYSTEM_ERROR:
                    this.statusCode = 500;
                    break;
                case ResponseCodes.USER_DOES_NOT_EXIST:
                    this.statusCode = 404;
                    break;
                default:
                    this.statusCode = 406;
                    break;
            }
        }
        public WebResponse<T> fail(string error) {
            this.Result = new StatusIdentifier { status = (int)ResponseCodes.GENERIC_ERROR, message = ResponseCodes.GENERIC_ERROR.ToString(), details = error };
            this.Content = default(T);
            this.statusCode = 400;
            return this;
        }
        public WebResponse<T> success(T content) {
            success();
            this.Content = content;
            return this;
        }
        public WebResponse<T> success(string message, T content) {
            success(message);
            this.Content = content;
            return this;
        }
        public WebResponse<T> success(string? message = null) {
            this.Result = new StatusIdentifier { status = (int)ResponseCodes.COMPLETED, message = message ?? ResponseCodes.COMPLETED.ToString() };
            return this;
        }
    }
    public class WebResponse : WebResponse<object> {

    }
    public class StatusIdentifier {
        public string message { get; set; }
        public int status { get; set; }
        public string? details { get; set; }
    }

    public enum ResponseCodes {
        COMPLETED = 0,
        USER_DOES_NOT_EXIST = 3,
        ACCESS_DENIED_ERROR = 4,
        CUSTOM_ERROR = 5,
        INVALID_REQUEST = 12,
        SYSTEM_ERROR = 20,
        LOGIC_ERROR = 10,
        INPUT_ERROR = 11,
        RESOURCE_NOT_FOUND = 7,
        SERVICE_ERROR = 30,
        GENERIC_ERROR = 5,
        ACTION_REQUIRED = 100
    }
}