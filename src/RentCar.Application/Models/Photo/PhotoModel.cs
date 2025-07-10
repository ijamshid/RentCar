namespace RentCar.Application.Models.Photo;

public class PhotoModel
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public int CarId { get; set; }
}