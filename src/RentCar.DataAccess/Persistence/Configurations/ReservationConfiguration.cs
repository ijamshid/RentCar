using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.CarId).IsRequired();

        // --- CHANGE FOR LOCAL TIMEZONE (timestamp without time zone) ---
        builder.Property(r => r.PickupDate)
            .HasColumnType("timestamp with time zone") // Explicitly set for local time
            .IsRequired();

        builder.Property(r => r.ReturnDate)
            .HasColumnType("timestamp with time zone") // Explicitly set for local time
            .IsRequired();
        // --- END CHANGE ---

        builder.Property(r => r.TotalPrice).HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>() // Store enum as string
            .HasMaxLength(50);

        builder.Property(u => u.CreatedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .IsRequired();

        builder.Property(r => r.LastModifiedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'"); // Nullable DateTime

        // Relationships are configured in CarConfiguration and UserConfiguration for clarity
        // Also explicitly configure the one-to-one relationship with Payment
        builder.HasOne(r => r.Payment)
            .WithOne(p => p.Reservation)
            .HasForeignKey<Payment>(p => p.ReservationId) // Payment's FK points to Reservation's PK
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting reservation if payment exists
    }
}
