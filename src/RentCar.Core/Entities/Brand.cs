using RentCar.Core.Common;

namespace RentCar.Core.Entities;

public class Brand:BaseEntity
{
    public string Name { get; set; }
    public string CountryOfOrigin { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public ICollection<Car> Cars { get; set; }
}
