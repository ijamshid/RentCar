using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.Core.Entities;
using RentCar.Application.Security;
using RentCar.Application.Security.AuthEnums;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;
using System.Reflection;
using UserRoleEnum = RentCar.Core.Enums.UserRole;


namespace RentCar.Application.Services.Implementation;

public class DataSeedService : IDataSeedService
{
    private readonly DatabaseContext _context;
    private readonly ILogger<DataSeedService> _logger;

    public DataSeedService(DatabaseContext context, ILogger<DataSeedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedRolesAndPermissionsAsync()
    {
        await SeedRolesAsync();
        await SeedPermissionGroupsAsync();
        await SeedPermissionsAsync();
        await SeedRolePermissionsAsync();
        await AssignAdminRoleToUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        _logger.LogInformation("Starting roles seeding...");

        // Get all enum values from UserRole
        var roleEnumValues = Enum.GetValues<UserRoleEnum>();

        foreach (var roleEnum in roleEnumValues)
        {
            var roleId = (int)roleEnum;
            var roleName = roleEnum.ToString();

            // Check if role exists
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (existingRole == null)
            {
                // Create new role
                var newRole = new Role
                {
                    Id = roleId,
                    Name = roleName,
                    Description=roleName,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Roles.AddAsync(newRole);
                _logger.LogInformation($"Added new role: {roleName}");
            }
            else if (existingRole.Name != roleName)
            {
                // Update role name if changed
                existingRole.Name = roleName;
                _logger.LogInformation($"Updated role name: {roleName}");
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Roles seeding completed.");
    }

    private async Task SeedPermissionGroupsAsync()
    {
        _logger.LogInformation("Starting permission groups seeding...");

        _context.ChangeTracker.Clear();

        var permissionGroupEnums = Enum.GetValues<ApplicationPermissionGroupCode>();

        foreach (var groupEnum in permissionGroupEnums)
        {
            var groupId = (int)groupEnum;
            var groupName = groupEnum.ToString();

            var existingGroup = await _context.PermissionGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(pg => pg.Id == groupId);

            if (existingGroup == null)
            {
                var newGroup = new PermissionGroup
                {
                    Id = groupId,
                    Name = groupName
                };

                await _context.PermissionGroups.AddAsync(newGroup);
                _logger.LogInformation($"Added new permission group: {groupName}");
            }
            else if (existingGroup.Name != groupName)
            {
                // Update requires attaching entity to context
                var groupToUpdate = new PermissionGroup { Id = groupId, Name = groupName };
                _context.PermissionGroups.Update(groupToUpdate);
                _logger.LogInformation($"Updated permission group name: {groupName}");
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Finished permission groups seeding.");
    }

    private async Task SeedPermissionsAsync()
    {
        _logger.LogInformation("Starting permissions seeding...");

        var permissionEnums = Enum.GetValues<ApplicationPermissionCode>();

        foreach (var permissionEnum in permissionEnums)
        {
            var permissionId = (int)permissionEnum;
            var permissionShortName = permissionEnum.ToString();

            var memberInfo = typeof(ApplicationPermissionCode).GetMember(permissionEnum.ToString()).FirstOrDefault();
            var attribute = memberInfo?.GetCustomAttribute<ApplicationPermissionDescriptionAttribute>();

            if (attribute == null)
                continue;

            var permissionGroupId = (int)attribute.ModulePermissionGroup;
            var permissionFullName = attribute.FullName;

            // Faqat 'Name' bo'yicha tekshiruv (unique bo'lishi kerak)
            var existingPermissionByName = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionFullName);

            if (existingPermissionByName != null)
            {
                // Agar mavjud bo'lsa, id ham tekshiring
                if (existingPermissionByName.Id != permissionId)
                {
                    // ID'lar mos kelmasa, bu konflikt bo'lishi mumkin
                    throw new InvalidOperationException($"Duplicate permission name detected with different ID: {permissionFullName}");
                }

                // Yangilash kerak bo'lsa (shortname yoki group o'zgargan bo'lsa)
                bool isUpdated = false;

                if (existingPermissionByName.ShortName != permissionShortName)
                {
                    existingPermissionByName.ShortName = permissionShortName;
                    isUpdated = true;
                }

                if (existingPermissionByName.PermissionGroupId != permissionGroupId)
                {
                    existingPermissionByName.PermissionGroupId = permissionGroupId;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    existingPermissionByName.UpdatedAt = DateTime.UtcNow;
                    _logger.LogInformation($"Updated permission: {permissionShortName}");
                }
            }
            else
            {
                // Yangi permission qo'shish
                var existingPermissionById = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Id == permissionId);

                if (existingPermissionById != null)
                {
                    // ID mavjud, lekin nomi boshqacha bo'lsa error
                    throw new InvalidOperationException($"Duplicate permission ID detected with different name: ID {permissionId}");
                }

                var newPermission = new Permission
                {
                    Id = permissionId,
                    ShortName = permissionShortName,
                    Name = permissionFullName,
                    Description = permissionFullName,
                    PermissionGroupId = permissionGroupId,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Permissions.AddAsync(newPermission);
                _logger.LogInformation($"Added new permission: {permissionShortName}");
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Permissions seeding completed.");
    }



    private async Task SeedRolePermissionsAsync()
    {
        _logger.LogInformation("Starting role-permission assignments...");

        var rolePermissions = new Dictionary<int, ApplicationPermissionCode[]>
        {
            [(int)UserRoleEnum.Admin] = Enum.GetValues<ApplicationPermissionCode>(),

            [(int)UserRoleEnum.User] = new[]
            {
            ApplicationPermissionCode.GetRole,
            ApplicationPermissionCode.GetCar,
            ApplicationPermissionCode.GetBrand,

            ApplicationPermissionCode.GetReservation,
            ApplicationPermissionCode.CreateReservation,
            ApplicationPermissionCode.CancelReservation,
            ApplicationPermissionCode.UpdateReservation,
            ApplicationPermissionCode.DeleteReservation,

            ApplicationPermissionCode.GetRating,
            ApplicationPermissionCode.CreateRating,
            ApplicationPermissionCode.UpdateRating,
            ApplicationPermissionCode.DeleteRating,

            ApplicationPermissionCode.GetPhoto,
            ApplicationPermissionCode.CreatePhoto,
            ApplicationPermissionCode.UpdatePhoto,
            ApplicationPermissionCode.DeletePhoto
        }
        };

        foreach (var kvp in rolePermissions)
        {
            var roleId = kvp.Key;
            var permissions = kvp.Value;

            foreach (var permission in permissions)
            {
                var permissionId = (int)permission;

                var exists = await _context.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

                if (!exists)
                {
                    var newRolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(newRolePermission);
                    _logger.LogInformation($"Added permission {permission} to role {roleId}");
                }
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Role-permission assignments completed.");
    }

    public async Task AssignAdminRoleToUserAsync()
    {
        var adminUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IsAdmin == true);

        if (adminUser == null)
        {
            _logger.LogWarning("Admin user not found.");
            return;
        }

        var adminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == nameof(UserRoleEnum.Admin));

        if (adminRole == null)
        {
            _logger.LogWarning("Admin role not found.");
            return;
        }

        var alreadyAssigned = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

        if (!alreadyAssigned)
        {
            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Admin role assigned to the user.");
        }
        else
        {
            _logger.LogInformation("Admin user already has the Admin role.");
        }
    }
}