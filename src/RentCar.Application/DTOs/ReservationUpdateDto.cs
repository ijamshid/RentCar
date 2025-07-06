using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class ReservationUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
    }
}
