namespace RentCar.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string email, string otpCode);
    }
}