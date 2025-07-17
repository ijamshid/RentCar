using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCar.Application.Extensions;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Models.Permissions;
using RentCar.Application.Security;
using RentCar.Application.Security.AuthEnums;
using RentCar.DataAccess.Persistence;
using System.Reflection;

namespace RentCar.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        public PermissionService(DatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<PermissionCodeDescription> GetAllPermissionDescriptions()
        {
            var permissions = new List<PermissionCodeDescription>();

            foreach (var field in typeof(ApplicationPermissionCode).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<ApplicationPermissionDescriptionAttribute>();

                if (attribute != null)
                {
                    permissions.Add(new PermissionCodeDescription
                    {
                        Code = field.Name,
                        ShortName = attribute.ShortName,
                        FullName = attribute.FullName
                    });
                }
            }
            return permissions;
        }

        public string GetPermissionShortName(ApplicationPermissionCode permissionCode)
        {
            FieldInfo? field = typeof(ApplicationPermissionCode).GetField(permissionCode.ToString());

            if (field != null)
            {
                var attribute = field.GetCustomAttribute<ApplicationPermissionDescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.ShortName;
                }
            }

            return permissionCode.ToString();
        }

        public async Task<ApiResult<List<PermissionGroupListModel>>> GetPermissionsFromDbAsync()
        {
            // 1. Permissionlarni bazaga sinxronlash (qo'shish/yangilash)
            await _dbContext.ResolvePermissions();

            // 2. Bazadan barcha permissionlarni guruhlari bilan birga yuklash
            var permissions = await _dbContext.Permissions
                .Include(p => p.PermissionGroup)
                .AsNoTracking()
                .ToListAsync();

            // 3. Olingan permissionlarni DTOlarga map qilish va guruhlash
            var groupedPermissions = permissions
                .GroupBy(p => p.PermissionGroup.Name) // PermissionGroup.Name orqali guruhlash
                .Select(g => new PermissionGroupListModel
                {
                    GroupName = g.Key,
                    Permissions = g.Select(p => new PermissionListModel
                    {
                        Id = p.Id,
                        ShortName = p.ShortName, // Permission.ShortName
                        FullName = p.Name,   // Permission.FullName
                        Description=p.Description,
                        GroupName = p.PermissionGroup.Name // PermissionGroup.Name
                    }).OrderBy(p => p.ShortName).ToList()
                })
                .OrderBy(pg => pg.GroupName)
                .ToList();

            return ApiResult<List<PermissionGroupListModel>>.Success(groupedPermissions);
        }
    }
}
