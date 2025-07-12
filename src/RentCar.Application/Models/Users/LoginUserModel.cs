using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users;

public class LoginUserModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; } = null!;
}
