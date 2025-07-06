using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CarId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();

            builder.Property(r => r.Stars)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(500); // Optional comment, max length 500

            builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .IsRequired();

            // Configure unique constraint if a user can only rate a car once
            builder.HasIndex(r => new { r.CarId, r.UserId }).IsUnique();

            // Relationships configured in CarConfiguration and UserConfiguration
        }
    }
}
