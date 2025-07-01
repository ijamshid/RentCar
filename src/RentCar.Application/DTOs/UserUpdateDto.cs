using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        [Required, MaxLength(100)]
        public string Address { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; } // Nullable DateTime

    }

}
