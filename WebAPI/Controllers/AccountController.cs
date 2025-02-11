using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Application.Interfaces.UseCases;
using Core.Domain.DTOs.Configurations;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAPI.ExceptionHelper;

namespace WebAPI.Controllers {
    public class AccountController : ControllerBase {
        private readonly SystemVariables _sysVar;
        private readonly IAccountUseCase _useCase;
        private readonly ExceptionMessage _errHandler;
        public AccountController(IOptionsMonitor<SystemVariables> sysVar, IAccountUseCase userUseCase, ILogger<AccountController> _logger, IIdentityManager _identity) {
            _sysVar = sysVar.CurrentValue;
            _useCase = userUseCase;
            _errHandler = new ExceptionMessage(_sysVar, _logger, _identity);
        }

        [HttpPost, Route("v1/join-waitlist")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> JoinWaitList([FromBody] WaitListRequest payload) {
            try {
                var response = await _useCase.JoinWaitList(payload.fullname, payload.email);
                return new ObjectResult(response) { StatusCode = response.statusCode };
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }
        public class WaitListRequest {
            public string fullname { get; set; }
            public string email { get; set; }
        }
    }
}
