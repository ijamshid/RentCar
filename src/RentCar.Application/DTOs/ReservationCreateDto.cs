using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class ReservationCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Status { get; set; } // Enum string value
    }

}
