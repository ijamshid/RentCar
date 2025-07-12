using Microsoft.EntityFrameworkCore;
using Rent.Core.Entities;
using RentCar.Core.Entities;
using SecureLoginApp.Core.Entities;
using System.Reflection;
namespace RentCar.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    { }
    public DbSet<UserOTPs> UserOTPs { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarPhoto> CarPhotos { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    //private static readonly DateTime adminDob = new DateTime(1980, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    //private static readonly DateTime createdAt = new DateTime(2025, 7, 9, 12, 0, 0, DateTimeKind.Utc);



    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Permission Groups
        builder.Entity<PermissionGroup>().HasData(
            new PermissionGroup { Id = 1, Name = "User Management" },
            new PermissionGroup { Id = 2, Name = "Role Management" },
            new PermissionGroup { Id = 3, Name = "Car Management" },
            new PermissionGroup { Id = 4, Name = "Brand Management" },
            new PermissionGroup { Id = 5, Name = "Reservation Management" },
            new PermissionGroup { Id = 6, Name = "Rating Management" },
            new PermissionGroup { Id = 7, Name = "Photo Management" },
            new PermissionGroup { Id = 8, Name = "Permission Management" }
        );

        // Permissions
        builder.Entity<Permission>().HasData(
            // User
            new Permission { Id = 1, Name = "UserCreate", Description = "User Create", ShortName = "UserCreate", PermissionGroupId = 1 },
            new Permission { Id = 2, Name = "UserRead", Description = "User Read", ShortName = "UserRead", PermissionGroupId = 1 },
            new Permission { Id = 3, Name = "UserUpdate", Description = "User Update", ShortName = "UserUpdate", PermissionGroupId = 1 },
            new Permission { Id = 4, Name = "UserDelete", Description = "User Delete", ShortName = "UserDelete", PermissionGroupId = 1 },
            new Permission { Id = 32, Name = "AddAdmin", Description = "Add Admin", ShortName = "AA", PermissionGroupId = 1 },
            // Role
            new Permission { Id = 5, Name = "GetRole", Description = "Get Role", ShortName = "GetRole", PermissionGroupId = 2 },
            new Permission { Id = 6, Name = "CreateRole", Description = "Create Role", ShortName = "CreateRole", PermissionGroupId = 2 },
            new Permission { Id = 7, Name = "UpdateRole", Description = "Update Role", ShortName = "UpdateRole", PermissionGroupId = 2 },
            new Permission { Id = 8, Name = "DeleteRole", Description = "Delete Role", ShortName = "DeleteRole", PermissionGroupId = 2 },
            // Car
            new Permission { Id = 9, Name = "GetCar", Description = "Get Car", ShortName = "GetCar", PermissionGroupId = 3 },
            new Permission { Id = 10, Name = "CreateCar", Description = "Create Car", ShortName = "CreateCar", PermissionGroupId = 3 },
            new Permission { Id = 11, Name = "UpdateCar", Description = "Update Car", ShortName = "UpdateCar", PermissionGroupId = 3 },
            new Permission { Id = 12, Name = "DeleteCar", Description = "Delete Car", ShortName = "DeleteCar", PermissionGroupId = 3 },
            // Brand
            new Permission { Id = 13, Name = "GetBrand", Description = "Get Brand", ShortName = "GetBrand", PermissionGroupId = 4 },
            new Permission { Id = 14, Name = "CreateBrand", Description = "Create Brand", ShortName = "CreateBrand", PermissionGroupId = 4 },
            new Permission { Id = 15, Name = "UpdateBrand", Description = "Update Brand", ShortName = "UpdateBrand", PermissionGroupId = 4 },
            new Permission { Id = 16, Name = "DeleteBrand", Description = "Delete Brand", ShortName = "DeleteBrand", PermissionGroupId = 4 },
            // Reservation
            new Permission { Id = 17, Name = "GetReservation", Description = "Get Reservation", ShortName = "GetReservation", PermissionGroupId = 5 },
            new Permission { Id = 18, Name = "CreateReservation", Description = "Create Reservation", ShortName = "CreateReservation", PermissionGroupId = 5 },
            new Permission { Id = 19, Name = "ConfirmReservation", Description = "Confirm Reservation", ShortName = "ConfirmReservation", PermissionGroupId = 5 },
            new Permission { Id = 20, Name = "CancelReservation", Description = "Cancel Reservation", ShortName = "CancelReservation", PermissionGroupId = 5 },
            new Permission { Id = 21, Name = "UpdateReservation", Description = "Update Reservation", ShortName = "UpdateReservation", PermissionGroupId = 5 },
            new Permission { Id = 22, Name = "DeleteReservation", Description = "Delete Reservation", ShortName = "DeleteReservation", PermissionGroupId = 5 },
            // Rating
            new Permission { Id = 23, Name = "GetRating", Description = "Get Rating", ShortName = "GetRating", PermissionGroupId = 6 },
            new Permission { Id = 24, Name = "CreateRating", Description = "Create Rating", ShortName = "CreateRating", PermissionGroupId = 6 },
            new Permission { Id = 25, Name = "UpdateRating", Description = "Update Rating", ShortName = "UpdateRating", PermissionGroupId = 6 },
            new Permission { Id = 26, Name = "DeleteRating", Description = "Delete Rating", ShortName = "DeleteRating", PermissionGroupId = 6 },
            // Photo
            new Permission { Id = 27, Name = "GetPhoto", Description = "Get Photo", ShortName = "GetPhoto", PermissionGroupId = 7 },
            new Permission { Id = 28, Name = "CreatePhoto", Description = "Create Photo", ShortName = "CreatePhoto", PermissionGroupId = 7 },
            new Permission { Id = 29, Name = "UpdatePhoto", Description = "Update Photo", ShortName = "UpdatePhoto", PermissionGroupId = 7 },
            new Permission { Id = 30, Name = "DeletePhoto", Description = "Delete Photo", ShortName = "DeletePhoto", PermissionGroupId = 7 },
            // Permission Management
            new Permission { Id = 31, Name = "GetPermissions", Description = "Get Permissions", ShortName = "GetPermissions", PermissionGroupId = 8 }
        );

        // Roles
        builder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Full system access" },
            new Role { Id = 2, Name = "User", Description = "Standard customer role" }
        );

        // Admin RolePermissions (barcha permissionlar)
        var adminRolePermissions = Enumerable.Range(1, 32).Select(i => new RolePermission
        {
            RoleId = 1,
            PermissionId = i
        }).ToArray();
        builder.Entity<RolePermission>().HasData(adminRolePermissions);

;
      


        int[] customerPermissions = {
    5, 9, 13, 17, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30
};
        var customerRolePermissions = customerPermissions.Select(pid => new RolePermission
        {
            RoleId = 2,
            PermissionId = pid
        }).ToArray();

        builder.Entity<RolePermission>().HasData(customerRolePermissions);
 
        base.OnModelCreating(builder);

        // So'ngra
    }
}
