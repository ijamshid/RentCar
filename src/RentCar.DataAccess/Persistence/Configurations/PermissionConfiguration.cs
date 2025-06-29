using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(p => p.Name).IsUnique(); // Permission names must be unique

            builder.Property(p => p.Description)
                .HasMaxLength(255);

            // Configure many-to-many with Role (through RolePermission)
            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting permission if roles are using it
        }
    }
}
