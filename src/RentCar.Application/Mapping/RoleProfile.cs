using AutoMapper;
using RentCar.Application.Models.Permissions;
using RentCar.Application.Models.Roles;
using RentCar.Core.Entities;

namespace RentCar.Application.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(p => new PermissionDto
                {
                    Id = p.PermissionId,
                    ShortName = p.Permission.ShortName
                })));
            CreateMap<Permission, PermissionDto>();
        }
    }
}
