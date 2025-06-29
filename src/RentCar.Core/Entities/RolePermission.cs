using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
   // Junction table for many-to-many relationship between Role and Permission
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
