using RentCar.Core.Common;
using RentCar.Core.Enums;

namespace RentCar.Core.Entities;

public class Car:BaseEntity
{
    public int BrandId { get; set; } // Foreign key to Brand
    public string Model { get; set; }
    public int Year { get; set; }
    public string PlateNumber { get; set; }
    public FuelType FuelType { get; set; }
    public decimal DailyPrice { get; set; }
    public int? Odometer { get; set; } // Nullable int
    public string Color { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Brand Brand { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Photo> Photos { get; set; } = new List<Photo>(); // New navigation property for Images

}
