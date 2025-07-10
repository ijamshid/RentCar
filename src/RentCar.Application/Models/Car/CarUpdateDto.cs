using RentCar.Core.Enums;

namespace RentCar.Application.Models.Car
{

    public class CarUpdateDto
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string PlateNumber { get; set; }

        public FuelType FuelType { get; set; }

        public decimal DailyPrice { get; set; }

        public int? Odometer { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }
    }
}
