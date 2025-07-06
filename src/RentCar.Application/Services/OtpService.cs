using Microsoft.EntityFrameworkCore;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;
using SecureLoginApp.Core.Entities;

namespace RentCar.Application.Services
{
    public class OtpService : IOtpService
    {
        private static readonly Dictionary<string, (string Code, DateTime Expires)> _otps = new();
        private readonly DatabaseContext context;
        private readonly IEmailService emailService;

        public OtpService(DatabaseContext context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
        }
        public async Task<string> GenerateOtpAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var otpCode = new Random().Next(100000, 999999).ToString();

            var otp = new UserOTPs
            {
                UserId = userId,
                Code = otpCode,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5)
            };

            await context.UserOTPs.AddAsync(otp);
            await context.SaveChangesAsync();

            await emailService.SendOtpAsync(user.Email, otpCode);
            return otpCode;
        }

        public async Task<UserOTPs?> GetLatestOtpAsync(int userId, string code)
        {
            return await context.UserOTPs
                .Where(o => o.UserId == userId && o.Code == code && o.ExpiredAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }


        public bool ValidateOtp(string email, string code)
        {
            if (_otps.TryGetValue(email, out var otp) && otp.Expires > DateTime.UtcNow && otp.Code == code)
            {
                _otps.Remove(email); // One-time use
                return true;
            }
            return false;
        }
    }
}
