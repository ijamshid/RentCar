using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;

namespace RentCar.DataAccess.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.ReservationId).IsRequired();
        builder.HasIndex(p => p.ReservationId).IsUnique(); // One-to-one relationship with Reservation means ReservationId must be unique in Payment table

        builder.Property(p => p.Amount).HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(p => p.PaymentDate)
             .IsRequired()
             .HasDefaultValueSql("NOW()");
        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(100);

        builder.Property(p => p.TransactionId)
            .HasMaxLength(255);
        builder.HasIndex(p => p.TransactionId).IsUnique(); // Transaction IDs should be unique

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        // Relationships are configured in UserConfiguration and ReservationConfiguration for clarity
    }
}
