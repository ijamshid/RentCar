using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Photo;

public class UpdatePhotoModel
{
    [Required(ErrorMessage = "Id maydoni to‘ldirilishi shart")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Url maydoni to‘ldirilishi shart")]
    public string Url { get; set; }
    public bool IsMain { get; set; } = false;
    [Required(ErrorMessage = "CarId maydoni to‘ldirilishi shart")]
    public int CarId { get; set; }
}