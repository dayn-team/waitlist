using Core.Application.Errors;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Others;
using Core.Domain.DTOs.Response;

namespace WebAPI.ExceptionHelper {
    public class ExceptionMessage {
        private readonly SystemVariables _var;
        private readonly ILogger _logger;
        private readonly IIdentityManager _identity;
        public int statusCode { get; private set; } = 200;
        WebResponse<object> response = new WebResponse<object>();
        public ExceptionMessage(SystemVariables _var, ILogger? logger, IIdentityManager id) {
            this._var = _var;
            _logger = logger;
            _identity = id;
        }
        public WebResponse<object> getMessage(Exception _exc) {
            if (_exc is AuthenticationError) {
                statusCode = 401;
                return response.fail(ResponseCodes.ACCESS_DENIED_ERROR, _exc.Message);
            }
            if (_exc is InputError) {
                var respObj = response.fail(ResponseCodes.INPUT_ERROR, _exc.Message);
                statusCode = respObj.statusCode;
                return respObj;
            }
            if (_exc is LogicError) {
                var respObj = response.fail(ResponseCodes.LOGIC_ERROR, _exc.Message);
                statusCode = respObj.statusCode;
                return respObj;
            }
            if (_exc is ServiceError) {
                var respObj = response.fail(ResponseCodes.SERVICE_ERROR, _exc.Message);
                statusCode = respObj.statusCode;
                return respObj;
            }
            if (_exc is NotFoundError) {
                var respObj = response.fail(ResponseCodes.RESOURCE_NOT_FOUND, _exc.Message);
                statusCode = respObj.statusCode;
                return respObj;
            }

            statusCode = 500;

            string deb = _identity.getHeaderValue("debug");
            if (_var.debug || !string.IsNullOrEmpty(deb)) {
                if (deb == "one") {
                    return response.fail(ResponseCodes.SYSTEM_ERROR, _exc.ToString());
                } else {
                    return response.fail(ResponseCodes.SYSTEM_ERROR, _exc.ToString());
                }
            }
            if (!(_logger is null)) {
                string route = _identity.endPointAddress;
                string request = _identity.IPAddress;
                var logData = new ErrLogDTO { details = _exc.ToString(), request = request, route = route };
                _logger.LogError("Request {route} Throws {@Position}", route, logData);
            }
            return response.fail(ResponseCodes.SYSTEM_ERROR, "Service is unavailable. Try again after some time or contact admin");
        }
    }
}
