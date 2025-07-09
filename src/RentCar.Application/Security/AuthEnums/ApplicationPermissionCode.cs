
namespace RentCar.Application.Security.AuthEnums;

[ApplicationEnumDescription]
public enum ApplicationPermissionCode
{
    #region Document

    #region User

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Create")]
    UserCreate,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Read")]
    UserRead,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Update")]
    UserUpdate,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Delete")]
    UserDelete,

    #endregion

    #region Permission

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Permission, "Get Permissions")]
    GetPermissions,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Permission, "Create Permissions")]
    CreatePermissions,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Permission, "Update Permissions")]
    UpdatePermissions,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Permission, "Delete Permissions")]
    DeletePermissions,

    #endregion
    #region Role

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Get Role")]
    GetRole,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Create Role")]
    CreateRole,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Update Role")]
    UpdateRole,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Delete Role")]
    DeleteRole,

    #endregion
    #region Car

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Get Car")]
    GetCar,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Create Car")]
    CreateCar,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Update Car")]
    UpdateCar,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Delete Car")]
    DeleteCar,

    #endregion
    #region Brand

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Get Brand")]
    GetBrand,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Create Brand")]
    CreateBrand,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Update Brand")]
    UpdateBrand,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Delete Brand")]
    DeleteBrand,

    #endregion
    #region Reservation

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Get Reservation")]
    GetReservation,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Create Reservation")]
    CreateReservation,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Update Reservation")]
    UpdateReservation,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Delete Reservation")]
    DeleteReservation,

    #endregion

    #region Rating

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Get Rating")]
    GetRating,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Create Rating")]
    CreateRating,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Update Rating")]
    UpdateRating,
    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Delete Rating")]
    DeleteRating,

    #endregion


    #endregion
}
