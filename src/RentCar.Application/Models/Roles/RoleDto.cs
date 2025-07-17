using RentCar.Application.Models.Permissions;

namespace RentCar.Application.Models.Roles
{
    public class RoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
