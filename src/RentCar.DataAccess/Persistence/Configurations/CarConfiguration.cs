using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.BrandId).IsRequired(); // Foreign key column is required

            builder.Property(c => c.Model)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Year)
                .IsRequired();

            builder.Property(c => c.PlateNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(c => c.PlateNumber).IsUnique(); // Creates a unique index

            builder.Property(c => c.FuelType)
                .IsRequired()
                .HasConversion<string>() // Stores the enum as its string representation in DB
                .HasMaxLength(50);

            builder.Property(c => c.DailyPrice).HasColumnType("decimal(10, 2)")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("text");


            builder.Property(c => c.Odometer); // Nullable int maps to nullable column by default

            builder.Property(c => c.Color)
                .HasMaxLength(50);


            builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

            // Configure relationships
            builder.HasOne(c => c.Brand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Reservations)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a car if reservations exist

            builder.HasMany(c => c.Ratings)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade); // Ratings can be deleted with the car
}
    }
}
