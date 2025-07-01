using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; } // Nullable DateTime

    }

}
