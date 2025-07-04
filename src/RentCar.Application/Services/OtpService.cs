using RentCar.Application.Services.Interfaces;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{
    public class OtpService : IOtpService
    {
        private static readonly Dictionary<string, (string Code, DateTime Expires)> _otps = new();

        public Task<string> GenerateOtpAsync(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            _otps[email] = (code, DateTime.UtcNow.AddMinutes(10));
            return Task.FromResult(code);
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
