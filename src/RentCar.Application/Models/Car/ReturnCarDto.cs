using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Car
{
    public class ReturnCarDto
    {
        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        public double CarMileage { get; set; }
    }
}
