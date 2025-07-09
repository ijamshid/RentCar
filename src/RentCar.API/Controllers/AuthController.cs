using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Users;
using RentCar.Application.Services;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ApiResult<string>> RegisterAsync([FromBody] RegisterUserModel model)
        {
            var result = await _authService.RegisterAsync(
                model.FirstName,
                model.LastName,
                model.Email,
                model.Password,
                model.isAdminSite,
                model.PhoneNumber,
                model.DateOfBirth);
            return result;
        }

        [HttpPost("login")]
        public async Task<ApiResult<LoginResponseModel>> LoginAsync([FromBody] LoginUserModel model)
        {
            var result = await _authService.LoginAsync(model);
            return result;
        }

        //// Emailga OTP yuborish uchun endpoint
        //[HttpPost("send-otp")]
        //public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestModel model)
        //{
        //    // model ichida userId va email bo'lishi kerak
        //    var otpCode = await _authService.SendOtpEmailAsync(model.Email);
        //    if (string.IsNullOrEmpty(otpCode))
        //        return BadRequest("OTP yuborishda xatolik yuz berdi.");

        //    return Ok(new { Message = "OTP yuborildi." });
        //}

        // OTPni tekshirish uchun endpoint
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestModel model)
        {
            bool isValid = await _authService.VerifyOtpAsync(model.Email, model.Code);
            if (!isValid)
                return BadRequest("Kod noto‘g‘ri yoki muddati tugagan.");

            return Ok("OTP muvaffaqiyatli tasdiqlandi.");
        }

        
        [HttpGet("is-authenticated")]
        public IActionResult IsAuthenticated()
        {
            return Ok(new
            {
                isAuthenticated = _authService.IsAuthenticated
            });
        }

        /// <summary>
        /// Foydalanuvchi ma’lumotlarini olish
        /// </summary>
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (!_authService.IsAuthenticated)
                return Unauthorized("Siz login qilmagansiz yoki tasdiqlanmagansiz.");

            var user = _authService.User;
            if (user == null)
                return NotFound("Foydalanuvchi topilmadi.");

            return Ok(user);
        }

        /// <summary>
        /// Foydalanuvchi permissionlarini olish
        /// </summary>
        [HttpGet("permissions")]
        public IActionResult GetPermissions()
        {
            if (!_authService.IsAuthenticated)
                return Unauthorized("Avval login qiling.");

            return Ok(_authService.Permissions);
        }

        /// <summary>
        /// Faqat ma’lum permission egalariga ochiq
        /// </summary>
        [HttpGet("secret-data")]
        public IActionResult GetSecretData()
        {
            if (!_authService.IsAuthenticated)
                return Unauthorized("Siz login qilmagansiz.");

            if (!_authService.HasPermission("VIEW_SECRET_DATA"))
                return Forbid("Sizda ushbu resursga ruxsat yo‘q.");

            return Ok(new
            {
                message = "Bu maxfiy ma’lumot faqat ruxsati bor foydalanuvchilar uchun."
            });
        }
    }

    // OTP yuborish uchun request model
    public class SendOtpRequestModel
    {
        public string Email { get; set; }
    }

    // OTP tekshirish uchun request model
    public class VerifyOtpRequestModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
