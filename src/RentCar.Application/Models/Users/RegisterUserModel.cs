using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class RegisterUserModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Tug‘ilgan sana kiritilishi shart.")]
        [DataType(DataType.Date, ErrorMessage = "Tug‘ilgan sana noto‘g‘ri formatda.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Telefon raqami kiritilishi shart.")]
        [Phone(ErrorMessage = "Telefon raqami noto‘g‘ri formatda.")]
        public string PhoneNumber { get; set; }

        public bool isAdminSite { get; set; }
    }
}
