using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface IUserRepository : IBaseRepository<User> {
        Task<List<User>> get(AccountFilter filter);
        Task<bool> accountExists(AccountFilter filter);
        Task<bool> loginUpdate(User user);
        Task<bool> updatePassword(string password, string username, int passwordChanged = 1);
    }
}
