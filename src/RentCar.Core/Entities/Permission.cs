using Rent.Core.Entities;
using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Permission:BaseEntity
    {
        public int Id { get; set; }
        public string ShortName { get; set; } = null!;
        public string Name { get; set; } // e.g., "ManageCars", "ViewReservations"
        public string Description { get; set; }

        // Navigation property
        public int PermissionGroupId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }


        public PermissionGroup PermissionGroup { get; set; } 

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
