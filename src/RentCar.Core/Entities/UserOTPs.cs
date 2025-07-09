using RentCar.Core.Entities;

namespace SecureLoginApp.Core.Entities;

public class UserOTPs
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Code { get; set; } = null!;
    public DateTime CreatedAt { get; set; } 
    public DateTime? ExpiredAt { get; set; }
}
