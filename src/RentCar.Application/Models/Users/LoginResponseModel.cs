using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class LoginResponseModel
    {
        [Required(ErrorMessage = "Email bo‘sh bo‘lishi mumkin emas.")]
        [EmailAddress(ErrorMessage = "Email formati noto‘g‘ri.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ism bo‘sh bo‘lishi mumkin emas.")]
        [MaxLength(50, ErrorMessage = "Ism 50 belgidan oshmasligi kerak.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Familiya bo‘sh bo‘lishi mumkin emas.")]
        [MaxLength(50, ErrorMessage = "Familiya 50 belgidan oshmasligi kerak.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Access token bo‘sh bo‘lishi mumkin emas.")]
        public string AccessToken { get; set; } = null!;

        [Required(ErrorMessage = "Refresh token bo‘sh bo‘lishi mumkin emas.")]
        public string RefreshToken { get; set; } = null!;

        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}