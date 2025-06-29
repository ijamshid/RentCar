using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Permission:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "ManageCars", "ViewReservations"
        public string Description { get; set; }

        // Navigation property
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
