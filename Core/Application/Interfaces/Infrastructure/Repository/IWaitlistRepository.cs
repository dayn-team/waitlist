using Core.Domain.Entities;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface IWaitlistRepository : IBaseRepository<Waitlist> {
        Task<bool> MailExists(string email);
    }
}
