using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Rating:BaseEntity
    {
        public int CarId { get; set; } // Foreign key to Car
        public int UserId { get; set; } // Foreign key to User
        public int Stars { get; set; } // e.g., 1 to 5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Car Car { get; set; }
        public User User { get; set; }
    }
}
