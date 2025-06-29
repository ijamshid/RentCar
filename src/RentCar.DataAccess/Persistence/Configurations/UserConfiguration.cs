using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Core.Entities;
namespace RentCar.DataAccess.Persistence.Configurations;


    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);


            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique(); // Email must be unique

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255); // Adjust length based on hashing algorithm output

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.Address)
                .HasMaxLength(500);

            builder.Property(u => u.DateOfBirth); // Nullable DateTime

        builder.Property(u => u.IsActive).HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
    .HasDefaultValueSql("NOW()");

        // Configure relationships
        builder.HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if active reservations exist

            builder.HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if payments exist

            builder.HasMany(u => u.Ratings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Ratings can be deleted with user

           }
    }
