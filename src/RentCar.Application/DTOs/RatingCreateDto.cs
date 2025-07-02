using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class RatingCreateDto
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }

}
