using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Users
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Ism kiritilishi shart.")]
        [MaxLength(50, ErrorMessage = "Ism 50 belgidan oshmasligi kerak.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Familiya kiritilishi shart.")]
        [MaxLength(50, ErrorMessage = "Familiya 50 belgidan oshmasligi kerak.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email kiritilishi shart.")]
        [EmailAddress(ErrorMessage = "Email formati noto‘g‘ri.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parol kiritilishi shart.")]
        [MinLength(8, ErrorMessage = "Parol kamida 8 ta belgidan iborat bo‘lishi kerak.")]
        [MaxLength(50, ErrorMessage = "Parol 50 belgidan oshmasligi kerak.")]
        public string Password { get; set; }
        private DateTime _dateOfBirth;

[Required(ErrorMessage = "Tug‘ilgan sana kiritilishi shart.")]
        [DataType(DataType.Date, ErrorMessage = "Tug‘ilgan sana noto‘g‘ri formatda.")]
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Required(ErrorMessage = "Telefon raqami kiritilishi shart.")]
        [Phone(ErrorMessage = "Telefon raqami noto‘g‘ri formatda.")]
        public string PhoneNumber { get; set; }

        // isAdminSite odatda checkbox yoki flag sifatida keladi, validatsiya kerak emas.
        public bool isAdminSite { get; set; }
    }
}