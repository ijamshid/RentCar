using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.DTOs
{
    public class RatingUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }

}
