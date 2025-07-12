using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class RegisterResponseModel : BaseResponseModel
    {
        [Required(ErrorMessage = "Email qiymati bo‘sh bo‘lishi mumkin emas.")]
        [EmailAddress(ErrorMessage = "Email noto‘g‘ri formatda.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Xabar (message) bo‘sh bo‘lishi mumkin emas.")]
        [MaxLength(200, ErrorMessage = "Xabar 200 belgidan oshmasligi kerak.")]
        public string Message { get; set; }
    }
}