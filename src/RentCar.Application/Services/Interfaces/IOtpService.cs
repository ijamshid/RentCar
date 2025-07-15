using SecureLoginApp.Core.Entities;

namespace RentCar.Application.Services.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email);
        bool ValidateOtp(string email, string code);
        Task<UserOTPs?> GetLatestOtpAsync(string email, string code);

    }
}
