using RentCar.Core.Enums;

namespace RentCar.Application.DTOs
{
    public class CarGetDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string PlateNumber { get; set; }
        public FuelType FuelType { get; set; }
        public decimal DailyPrice { get; set; }
        public int? Odometer { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string>? ImageUrls { get; set; } = new();
    }
}
