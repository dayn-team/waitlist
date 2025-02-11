using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;

namespace Core.Application.Interfaces.UseCases {
    public interface IAccountUseCase {
        Task<WebResponse<object>> login(string username, string password);
        Task<WebResponse<object>> createAccount(UserSignupDTO account);
        Task<WebResponse<object>> updatePassword(string password);
        Task<WebResponse<object>> updateAccount(UserSignupDTO account);
        Task<WebResponse<object>> retrievePassword(string username, string password);
        Task<WebResponse<object>> verifyAccount(string code);
        Task<WebResponse<object>> resetPassword(string username, string email);
        Task<WebResponse<object>> JoinWaitList(string fullname, string email);
    }
}