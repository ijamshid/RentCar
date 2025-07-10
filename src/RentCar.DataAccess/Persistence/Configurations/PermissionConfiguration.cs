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

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.Property(p => p.ShortName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(255);

            // CreatedAt uchun default qiymat (agar kerak bo'lsa)
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            // UpdatedAt maydonini update paytida avtomatik yangilanishi uchun sozlash
            builder.Property(p => p.UpdatedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .IsConcurrencyToken(false)
                .IsRequired(false);

            // Foreign key va navigatsiyalar
            builder.HasOne(p => p.PermissionGroup)
                .WithMany(pg => pg.Permissions)
                .HasForeignKey(p => p.PermissionGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
