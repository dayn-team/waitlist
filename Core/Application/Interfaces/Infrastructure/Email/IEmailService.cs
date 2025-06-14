using Core.Domain.DTOs.Others;

namespace Core.Application.Interfaces.Infrastructure.Email {
    public interface IEmailService {
        Task<bool> Send(MailEnvelope envelope);
    }
}
