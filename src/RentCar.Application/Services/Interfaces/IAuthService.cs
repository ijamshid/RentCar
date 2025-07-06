using RentCar.Application.Security;

namespace RentCar.Application.Services;

public interface IAuthService
{
    IUser User { get; }
    int GetUserId();
    bool IsAuthenticated { get; }
    HashSet<string> Permissions { get; }
    bool HasPermission(params string[] permissionCodes);
}
