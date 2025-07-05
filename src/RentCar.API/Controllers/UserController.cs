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
            var result = await _userService.RegisterAsync(model.FirstName, model.LastName,model.Email, model.Password, model.isAdminSite, model.PhoneNumber, model.DateOfBirth);
            return result;
        }

        [HttpPost("login")]
        public async Task<ApiResult<LoginResponseModel>> LoginAsync([FromBody] LoginUserModel model)
        {
            var result = await _userService.LoginAsync(model);
            return result;
        }

        [HttpPost("verify-otp")]
        public async Task<ApiResult<string>> VerifyOtpAsync([FromBody] OtpVerificationModel model)
        {
            var result = await _userService.VerifyOtpAsync(model);
            return result;
        }

        [Authorize]
        [HttpGet("get-user-auth")]
        public async Task<IActionResult> GetUserAuth()
        {
            var result = await _userService.GetUserAuth();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
