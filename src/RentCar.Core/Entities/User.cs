using RentCar.Core.Common;

namespace RentCar.Core.Entities;

public class User : BaseEntity
{
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public string Address { get; set; }
    public bool IsVerified { get; set; } = false;

    public string? RefreshToken { get; set; }
    // Navigation properties
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public DateTime? DateOfBirth { get; set; } // Nullable DateTime
    public DateTime CreatedAt { get; set; }


}
