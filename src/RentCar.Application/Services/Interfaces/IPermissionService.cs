using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Permissions;
using RentCar.Application.Security;
using RentCar.Application.Security.AuthEnums;

namespace RentCar.Application.Services;

public interface IPermissionService
{
    List<PermissionCodeDescription> GetAllPermissionDescriptions();
    string GetPermissionShortName(ApplicationPermissionCode permissionCode);
    Task<ApiResult<List<PermissionGroupListModel>>> GetPermissionsFromDbAsync();
}
