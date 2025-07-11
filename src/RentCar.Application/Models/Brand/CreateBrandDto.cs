using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Brand
{
    public class CreateBrandDto
    {
        [Required(ErrorMessage = "Brand nomi majburiy")]
        [StringLength(100, ErrorMessage = "Brand nomi 100 ta belgidan oshmasligi kerak")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kelib chiqqan davlati majburiy")]
        [StringLength(50, ErrorMessage = "Mamlakat nomi 50 ta belgidan oshmasligi kerak")]
        public string CountryOfOrigin { get; set; }
    }
}
