using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Users;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ApiResult<string>> RegisterAsync([FromBody] RegisterUserModel model)
        {
            var result = await _userService.RegisterAsync(
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
            var result = await _userService.LoginAsync(model);
            return result;
        }

        // Emailga OTP yuborish uchun endpoint
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestModel model)
        {
            // model ichida userId va email bo'lishi kerak
            var otpCode = await _userService.SendOtpEmailAsync(model.Email);
            if (string.IsNullOrEmpty(otpCode))
                return BadRequest("OTP yuborishda xatolik yuz berdi.");

            return Ok(new { Message = "OTP yuborildi." });
        }

        // OTPni tekshirish uchun endpoint
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestModel model)
        {
            bool isValid = await _userService.VerifyOtpAsync(model.UserId, model.Code);
            if (!isValid)
                return BadRequest("Kod noto‘g‘ri yoki muddati tugagan.");

            return Ok("OTP muvaffaqiyatli tasdiqlandi.");
        }

        [Authorize]
        [HttpGet("get-user-auth")]
        public async Task<IActionResult> GetUserAuth()
        {
            var result = await _userService.GetUserAuth();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
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
        public int UserId { get; set; }
        public string Code { get; set; }
    }
}
