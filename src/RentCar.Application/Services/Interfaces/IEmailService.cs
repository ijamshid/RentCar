namespace RentCar.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string toEmail, string otp);
    }
}