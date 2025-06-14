using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;

namespace Core.Application.Interfaces.UseCases {
    public interface IAccountUseCase {
        Task<WebResponse<object>> Login(string username, string password);
        Task<WebResponse<object>> CreateAccount(UserSignupDTO account);
        Task<WebResponse<object>> UpdatePassword(string password);
        Task<WebResponse<object>> UpdateAccount(UserSignupDTO account);
        Task<WebResponse<object>> RetrievePassword(string username, string password);
        Task<WebResponse<object>> VerifyAccount(string code);
        Task<WebResponse<object>> ResetPassword(string username, string email);
        Task<WebResponse<object>> JoinWaitList(string fullname, string email);
    }
}