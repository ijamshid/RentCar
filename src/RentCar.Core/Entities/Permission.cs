using Rent.Core.Entities;
using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Permission:BaseEntity
    {
        public string ShortName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;  // Bazada NOT NULL bo‘lsa

        // Navigation property
        public int PermissionGroupId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
       public PermissionGroup PermissionGroup { get; set; } 

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

