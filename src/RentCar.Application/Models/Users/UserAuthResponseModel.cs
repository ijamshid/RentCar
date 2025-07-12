using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class UserAuthResponseModel
    {
        [Required(ErrorMessage = "Foydalanuvchi IDsi bo‘sh bo‘lishi mumkin emas.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Foydalanuvchi to‘liq ismi bo‘sh bo‘lishi mumkin emas.")]
        [MaxLength(100, ErrorMessage = "To‘liq ism 100 belgidan oshmasligi kerak.")]
        public string FullName { get; set; }
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
    }
}