    using RentCar.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace Rent.Core.Entities;

    public class PermissionGroup
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
