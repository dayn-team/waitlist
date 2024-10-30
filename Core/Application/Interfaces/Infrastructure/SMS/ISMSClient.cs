namespace Core.Application.Interfaces.Infrastructure.SMS {
    public interface ISMSClient {
        Task<bool> sendSMS(string message, string phone);
        Task<string> sendOTP(string phone);
        Task<bool> verifyOTP(string phone, string otp, string token);
    }
}
