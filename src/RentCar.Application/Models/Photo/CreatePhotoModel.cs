using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Photo
{
    public class CreatePhotoModel
    {
        public int Id { get; set; } // Optional: odatda Create uchun kerak emas

        [Required(ErrorMessage = "Rasm URL manzili majburiy")]
        [StringLength(300, ErrorMessage = "URL manzili 300 ta belgidan oshmasligi kerak")]
        [Url(ErrorMessage = "To‘g‘ri URL manzil kiriting")]
        public string Url { get; set; }

        public bool IsMain { get; set; } = false;

        [Required(ErrorMessage = "CarId majburiy")]
        public int CarId { get; set; }
    }
}
