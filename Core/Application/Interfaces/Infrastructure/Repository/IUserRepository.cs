using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface IUserRepository : IBaseRepository<User> {
        Task<List<User>> Get(AccountFilter filter);
        Task<bool> AccountExists(AccountFilter filter);
        Task<bool> LoginUpdate(User user);
        Task<bool> UpdatePassword(string password, string username, int passwordChanged = 1);
    }
}
