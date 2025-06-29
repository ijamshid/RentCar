using RentCar.Core.Common;

namespace RentCar.Core.Entities
{
    public class Image:BaseEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; } // Foreign key to Car
        public string Url { get; set; }
        public string AltText { get; set; } // Alternative text for accessibility/SEO
        public int Order { get; set; } // To define display order
        public DateTime UploadedAt { get; set; }

        // Navigation property
        public Car Car { get; set; }
    }
}
