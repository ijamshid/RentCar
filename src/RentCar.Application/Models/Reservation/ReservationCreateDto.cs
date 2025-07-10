using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Reservation
{
    public class ReservationCreateDto
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

    }
}
