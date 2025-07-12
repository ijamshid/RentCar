using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Email kiritilishi shart.")]
        [EmailAddress(ErrorMessage = "Email formati noto‘g‘ri.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Parol kiritilishi shart.")]
        [MinLength(8, ErrorMessage = "Parol kamida 8 ta belgidan iborat bo‘lishi kerak.")]
        [MaxLength(50, ErrorMessage = "Parol 50 ta belgidan oshmasligi kerak.")]
        public string Password { get; set; } = null!;
    }
}