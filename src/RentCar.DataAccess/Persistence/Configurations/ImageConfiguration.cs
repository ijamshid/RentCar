using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.CarId).IsRequired();

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(2048); // Standard URL max length

            builder.Property(i => i.AltText)
                .HasMaxLength(255);

            builder.Property(i => i.Order)
                .IsRequired();

            builder.Property(i => i.UploadedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .IsRequired();

            // Configure relationship with Car
            //builder.HasOne(i => i.Car)
            //    .WithMany(c => c.)
            //    .HasForeignKey(i => i.CarId)
            //    .OnDelete(DeleteBehavior.Cascade); // If car is deleted, its images are deleted
        }
    }
}
