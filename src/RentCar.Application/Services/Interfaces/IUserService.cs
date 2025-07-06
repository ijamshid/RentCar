using RentCar.Application.DTOs;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Users;

namespace RentCar.Application.Services.Interfaces
{

    public interface IUserService
    {
        Task<IEnumerable<UserGetDto>> GetAllAsync();
        Task<UserGetDto> GetByIdAsync(int id);
        Task<UserGetDto> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ApiResult<string>> RegisterAsync(string firstname, string lastname, string email, string password, bool isAdminSite, string a, DateTime b);
        Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model);
        Task<ApiResult<string>> VerifyOtpAsync(OtpVerificationModel model);
        Task<ApiResult<UserAuthResponseModel>> GetUserAuth();
    }
}
