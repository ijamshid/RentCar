using SecureLoginApp.Core.Entities;

namespace RentCar.Application.Services.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(int userId);
        bool ValidateOtp(string email, string code);
        Task<UserOTPs?> GetLatestOtpAsync(int userId, string code);

    }
}
