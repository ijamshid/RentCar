using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Role:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Admin", "Customer", "Staff"
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}