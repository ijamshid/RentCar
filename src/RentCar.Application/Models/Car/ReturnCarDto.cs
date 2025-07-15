using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Car
{
    public class ReturnCarDto
    {
        [Required(ErrorMessage = "Qaytarish sanasi majburiy")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Avtomobilning yurgan masofasi majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Avtomobilning yurgan masofasi manfiy bo‘lishi mumkin emas")]
        public double CarMileage { get; set; }
    }
}
