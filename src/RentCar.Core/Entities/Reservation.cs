
using RentCar.Core.Common;
using RentCar.Core.Enums;

namespace RentCar.Core.Entities;

public class Reservation:BaseEntity
{
    public int UserId { get; set; } // Foreign key to User
    public int CarId { get; set; } // Foreign key to Car
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; } // Nullable DateTime for updates

    // Navigation properties
    public User User { get; set; }
    public Car Car { get; set; }
    public Payment Payment { get; set; } // One-to-one with Payment
}
