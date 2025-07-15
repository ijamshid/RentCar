using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Photo;

public class ResponsePhotoModel
{
   public bool IsSuccess { get; set; }
    [Required]
   public string Status { get; set; }
}