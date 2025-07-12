namespace RentCar.Core.Entities
{
    public class CarPhoto
    {
        public int Id { get; set; }
        public string ObjectName { get; set; } // instead of Url
        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
