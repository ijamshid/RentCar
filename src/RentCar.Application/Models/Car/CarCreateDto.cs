using RentCar.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Car
{
    public class CarCreateDto
    {
        [Required(ErrorMessage = "BrandId majburiy")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Model nomi majburiy")]
        [StringLength(100, ErrorMessage = "Model nomi 100 ta belgidan oshmasligi kerak")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Yil majburiy")]
        [Range(1950, 2100, ErrorMessage = "Yil 1950 va 2100 orasida bo‘lishi kerak")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Davlat raqami majburiy")]
        [StringLength(10, ErrorMessage = "Davlat raqami 10 ta belgidan oshmasligi kerak")]
        [RegularExpression(@"^(\d{2}[A-Z]\d{3}[A-Z]{2}|\d{5}[A-Z]{3})$",
            ErrorMessage = "Davlat raqami formati noto‘g‘ri. Ruxsat etilgan: '01A123AA' yoki '01012AAA'")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Yoqilg‘i turi majburiy")]
        public FuelType FuelType { get; set; }

        [Required(ErrorMessage = "Kunlik narx majburiy")]
        [Range(0, 10000, ErrorMessage = "Kunlik narx manfiy bo‘lishi mumkin emas")]
        public decimal DailyPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Odometr qiymati manfiy bo‘lishi mumkin emas")]
        public int? Odometer { get; set; }

        [StringLength(30, ErrorMessage = "Rang nomi 30 ta belgidan oshmasligi kerak")]
        public string Color { get; set; }

        [StringLength(500, ErrorMessage = "Tavsif 500 ta belgidan oshmasligi kerak")]
        public string Description { get; set; }
    }
}
