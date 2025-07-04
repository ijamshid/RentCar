using RentCar.Application.Services.Interfaces;

namespace RentCar.Application.Services
{
    public class EmailService : IEmailService
    {
        public Task SendOtpAsync(string email, string otpCode)
        {
            Console.WriteLine($"[OTP] To {email}: {otpCode}");
            return Task.CompletedTask;
        }
    }

}
