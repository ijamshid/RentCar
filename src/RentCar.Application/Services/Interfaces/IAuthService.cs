using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Users;
using RentCar.Application.Security;

namespace RentCar.Application.Services;
public interface IAuthService
{
    IUser User { get; }
    int GetUserId();
    bool IsAuthenticated { get; }
    HashSet<string> Permissions { get; }
    bool HasPermission(params string[] permissionCodes);

    Task<bool> VerifyOtpAsync(string email, string inputCode);

    Task<ApiResult<string>> RegisterAsync(string firstname, string lastname, string email, string password, bool isAdminSite, string a, DateTime b);
    Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model);
}
