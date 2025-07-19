namespace RentCar.Application.Security.AuthEnums;

[ApplicationEnumDescription]
public enum ApplicationPermissionCode
{
    #region Document

    #region User

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Create")]
    UserCreate = 1,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "Add Admin")]
    AddAdmin = 32,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Read")]
    UserRead = 2,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Update")]
    UserUpdate = 3,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.User, "User Delete")]
    UserDelete = 4,

    #endregion

    #region Permission

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Permission, "Get Permissions")]
    GetPermissions = 31,

    #endregion

    #region Role

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Get Role")]
    GetRole = 5,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Create Role")]
    CreateRole = 6,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Update Role")]
    UpdateRole = 7,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Role, "Delete Role")]
    DeleteRole = 8,

    #endregion

    #region Car

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Get Car")]
    GetCar = 9,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Create Car")]
    CreateCar = 10,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Update Car")]
    UpdateCar = 11,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Car, "Delete Car")]
    DeleteCar = 12,

    #endregion

    #region Brand

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Get Brand")]
    GetBrand = 13,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Create Brand")]
    CreateBrand = 14,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Update Brand")]
    UpdateBrand = 15,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Brand, "Delete Brand")]
    DeleteBrand = 16,

    #endregion

    #region Reservation

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Get Reservation")]
    GetReservation = 17,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Create Reservation")]
    CreateReservation = 18,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Confirm Reservation")]
    ConfirmReservation = 19,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Cancel Reservation")]
    CancelReservation = 20,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Update Reservation")]
    UpdateReservation = 21,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Reservation, "Delete Reservation")]
    DeleteReservation = 22,

    #endregion

    #region Rating

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Get Rating")]
    GetRating = 23,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Create Rating")]
    CreateRating = 24,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Update Rating")]
    UpdateRating = 25,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Rating, "Delete Rating")]
    DeleteRating = 26,

    #endregion

    #region Photo

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Photo, "Get Photo")]
    GetPhoto = 27,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Photo, "Create Photo")]
    CreatePhoto = 28,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Photo, "Update Photo")]
    UpdatePhoto = 29,

    [ApplicationPermissionDescription(ApplicationPermissionGroupCode.Photo, "Delete Photo")]
    DeletePhoto = 30,

    #endregion

    #endregion
}
