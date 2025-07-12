using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Rating
{
    public class RatingUpdateDto
    {
        [Required(ErrorMessage = "Reyting IDsi kiritilishi shart.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yulduzcha soni (reyting qiymati) kiritilishi shart.")]
        [Range(1, 5, ErrorMessage = "Yulduzcha soni 1 dan 5 gacha bo‘lishi kerak.")]
        public int Stars { get; set; }

        [MaxLength(500, ErrorMessage = "Kommentariya 500 belgidan oshmasligi kerak.")]
        public string Comment { get; set; }
    }
}