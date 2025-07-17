using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(b => b.Name).IsUnique(); // Ensure brand names are unique

            builder.Property(b => b.CountryOfOrigin)
                .HasMaxLength(100);

            builder.Property(u => u.CreatedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .IsRequired();

            // Configure one-to-many relationship with Car
            builder.HasMany(b => b.Cars)
                .WithOne(c => c.Brand)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Cascade); // Prevent deleting a brand if cars are associated
        }
    }
}
