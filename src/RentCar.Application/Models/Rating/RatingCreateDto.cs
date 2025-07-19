using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Rating
{
    public class RatingCreateDto
    {
        [Required(ErrorMessage = "Rezervatsiya ID kiritilishi shart.")]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Reyting qiymati kiritilishi shart.")]
        [Range(1, 5, ErrorMessage = "Reyting qiymati 1 dan 5 gacha bo‘lishi kerak.")]
        public int Value { get; set; }

        [MaxLength(500, ErrorMessage = "Kommentariya 500 belgidan oshmasligi kerak.")]
        public string Comment { get; set; }
    }
}