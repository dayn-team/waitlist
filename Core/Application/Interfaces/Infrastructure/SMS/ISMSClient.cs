namespace Core.Application.Interfaces.Infrastructure.SMS {
    public interface ISMSClient {
        Task<bool> SendSMS(string message, string phone);
        Task<string> SendOTP(string phone);
        Task<bool> VerifyOTP(string phone, string otp, string token);
    }
}
