using RentalHome.Core.Common;

namespace RentalHome.Core.Entities;

public class User : BaseEntity
{
    public string PasswordHash { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string FullName { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public string Salt { get; set; }
    public string? RefreshToken { get; set; }
}
