using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users;

public class UserAuthResponseModel
{
    [Required(ErrorMessage = "Foydalanuvchi id bo'sh bo'lmasligi kerak")]
    public int Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }
    public IEnumerable<string> Permissions { get; set; } = new List<string>();
}
