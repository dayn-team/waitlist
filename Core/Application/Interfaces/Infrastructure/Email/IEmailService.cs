using Core.Domain.DTOs.Others;

namespace Core.Application.Interfaces.Email {
    public interface IEmailService {
        Task<bool> send(MailEnvelope envelope);
    }
}
