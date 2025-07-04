namespace RentCar.Application.Services.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email);
        bool ValidateOtp(string email, string code);
    }
}
