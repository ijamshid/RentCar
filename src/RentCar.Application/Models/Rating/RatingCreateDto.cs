using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Rating
{
    public class RatingCreateDto
    {
        [Required(ErrorMessage = "Id maydoni to'ldirilishi shart")]
        public int ReservationId { get; set; }
        [Required(ErrorMessage = "Id maydoni to'ldirilishi shart")]
        public int CarId { get; set; }
        [Required(ErrorMessage = "Qiymat bo'sh bo'lishi mumkin emas")]
        [Range(1, 5, ErrorMessage = "Qiymat 1 dan 5 gacha bo'lishi kerak")]
        public int Value { get; set; }
        [MaxLength(500, ErrorMessage = "Comment 300 belgidan oshmasligi kerak")]
        public string Comment { get; set; }
    }


}
