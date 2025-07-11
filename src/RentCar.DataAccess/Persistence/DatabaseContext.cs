using Microsoft.EntityFrameworkCore;
using RentCar.Core.Entities;
using SecureLoginApp.Core.Entities;
using System.Reflection;
namespace RentCar.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    { }
    public DbSet<UserOTPs> UserOTPs { get; set; }

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
    //private static readonly DateTime adminDob = new DateTime(1980, 1, 1,12,0,0, DateTimeKind.Utc);
    //private static readonly DateTime createdAt = new DateTime(2025, 7, 9, 12, 0, 0, DateTimeKind.Utc);



    protected override void OnModelCreating(ModelBuilder builder)
    {


  //      // Roles
  //      builder.Entity<Role>().HasData(
  //          new Role { Id = 1, Name = "Admin", Description = "Administrator role with full access" },
  //          new Role { Id = 2, Name = "Customer", Description = "Default role for regular users" },
  //          new Role { Id = 3, Name = "Staff", Description = "Staff role for managing cars and reservations" }
  //      );

  //      // Permissions
  //      builder.Entity<Permission>().HasData(
  //    // User
  //    new Permission { Id = 1, Name = "UserCreate", Description = "User Create", ShortName = "UserCreate" },
  //    new Permission { Id = 2, Name = "UserRead", Description = "User Read", ShortName = "UserRead" },
  //    new Permission { Id = 3, Name = "UserUpdate", Description = "User Update", ShortName = "UserUpdate" },
  //    new Permission { Id = 4, Name = "UserDelete", Description = "User Delete", ShortName = "UserDelete" },

  //    // Permission
  //    new Permission { Id = 5, Name = "GetPermissions", Description = "Get Permissions", ShortName = "GetPermissions" },
  //    new Permission { Id = 6, Name = "CreatePermissions", Description = "Create Permissions", ShortName = "CreatePermissions" },
  //    new Permission { Id = 7, Name = "UpdatePermissions", Description = "Update Permissions", ShortName = "UpdatePermissions" },
  //    new Permission { Id = 8, Name = "DeletePermissions", Description = "Delete Permissions", ShortName = "DeletePermissions" },

  //    // Role
  //    new Permission { Id = 9, Name = "GetRole", Description = "Get Role", ShortName = "GetRole" },
  //    new Permission { Id = 10, Name = "CreateRole", Description = "Create Role", ShortName = "CreateRole" },
  //    new Permission { Id = 11, Name = "UpdateRole", Description = "Update Role", ShortName = "UpdateRole" },
  //    new Permission { Id = 12, Name = "DeleteRole", Description = "Delete Role", ShortName = "DeleteRole" },

  //    // Car
  //    new Permission { Id = 13, Name = "GetCar", Description = "Get Car", ShortName = "GetCar" },
  //    new Permission { Id = 14, Name = "CreateCar", Description = "Create Car", ShortName = "CreateCar" },
  //    new Permission { Id = 15, Name = "UpdateCar", Description = "Update Car", ShortName = "UpdateCar" },
  //    new Permission { Id = 16, Name = "DeleteCar", Description = "Delete Car", ShortName = "DeleteCar" },

  //    // Brand
  //    new Permission { Id = 17, Name = "GetBrand", Description = "Get Brand", ShortName = "GetBrand" },
  //    new Permission { Id = 18, Name = "CreateBrand", Description = "Create Brand", ShortName = "CreateBrand" },
  //    new Permission { Id = 19, Name = "UpdateBrand", Description = "Update Brand", ShortName = "UpdateBrand" },
  //    new Permission { Id = 20, Name = "DeleteBrand", Description = "Delete Brand", ShortName = "DeleteBrand" },

  //    // Reservation
  //    new Permission { Id = 21, Name = "GetReservation", Description = "Get Reservation", ShortName = "GetReservation" },
  //    new Permission { Id = 22, Name = "CreateReservation", Description = "Create Reservation", ShortName = "CreateReservation" },
  //    new Permission { Id = 23, Name = "UpdateReservation", Description = "Update Reservation", ShortName = "UpdateReservation" },
  //    new Permission { Id = 24, Name = "DeleteReservation", Description = "Delete Reservation", ShortName = "DeleteReservation" },

  //    // Rating
  //    new Permission { Id = 25, Name = "GetRating", Description = "Get Rating", ShortName = "GetRating" },
  //    new Permission { Id = 26, Name = "CreateRating", Description = "Create Rating", ShortName = "CreateRating" },
  //    new Permission { Id = 27, Name = "UpdateRating", Description = "Update Rating", ShortName = "UpdateRating" },
  //    new Permission { Id = 28, Name = "DeleteRating", Description = "Delete Rating", ShortName = "DeleteRating" }
  //);


  //      // Admin uchun barcha permissions (1 dan 28 gacha)
  //      var adminPermissions = Enumerable.Range(1, 28)
  //          .Select(id => new RolePermission
  //          {
  //              RoleId = 1,
  //              PermissionId = id
  //          })
  //          .ToArray();

  //      builder.Entity<RolePermission>().HasData(adminPermissions);

  //      // Customer uchun permissions (misol: 5,9,13,17,21,22,23,24,25,26,27,28)
  //      int[] customerPermissionIds = { 5, 9, 13, 17, 21, 22, 23, 24, 25, 26, 27, 28 };
  //      var customerPermissions = customerPermissionIds.Select(id => new RolePermission
  //      {
  //          RoleId = 2,
  //          PermissionId = id
  //      }).ToArray();

  //      builder.Entity<RolePermission>().HasData(customerPermissions);

  //      // Staff uchun permissions (avvalgi kabi)
  //      int[] staffPermissionIds = { 2, 13, 14, 15, 16, 21, 23, 24, 8, 25, 26, 27, 28, 17, 18, 19, 20 };
  //      var staffPermissions = staffPermissionIds.Select(id => new RolePermission
  //      {
  //          RoleId = 3,
  //          PermissionId = id
  //      }).ToArray();

  //      builder.Entity<RolePermission>().HasData(staffPermissions);

    //    string hash = "f2P+NdhTXkWiPo+5GiJf/9t1XjsYXOO9q1hE6ZQkvzE="; // Placeholder - replace with your actual generated Base64 hash
    //    string salt = "a3f1d1e8-cd55-4b3c-9a12-8c3b3f9b2e4a"; // Placeholder - replace with your actual generated Base64 salt

    //builder.Entity<User>().HasData(
    //        new User
    //        {
    //            Id = 1,
    //            Firstname = "Jamshid",
    //            Lastname = "Ismoilov",
    //            Email = "admin@carrental.com",
    //            PasswordHash = hash,
    //            Salt = salt,
    //            PhoneNumber = "555-123-4567",
    //            DateOfBirth = adminDob,
    //            IsActive = true,
    //            IsVerified = true,
    //            CreatedAt = createdAt
    //        }
    //    );
        //Assign Admin Role to Primary Admin User
        //builder.Entity<UserRole>().HasData(
        //     new UserRole { UserId = 1, RoleId = 1 } // Link Primary Admin (Id 1) to Admin Role (Id 1)
        // );



        base.OnModelCreating(builder);

        // So'ngra
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
