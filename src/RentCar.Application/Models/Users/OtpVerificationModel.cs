using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users;

public class OtpVerificationModel
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Belgilar soni 6 tadan kam bo'lmasligi kerak")]
    public string Code { get; set; }
}
