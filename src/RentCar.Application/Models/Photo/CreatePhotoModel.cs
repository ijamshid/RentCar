namespace RentCar.Application.Models.Photo;

public class CreatePhotoModel
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; } = false;
    public int CarId { get; set; }
}