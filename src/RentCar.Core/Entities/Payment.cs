using RentCar.Core.Common;
using RentCar.Core.Enums;

namespace RentCar.Core.Entities;

public class Payment:BaseEntity
{
    public int UserId { get; set; } // Foreign key to User
    public int ReservationId { get; set; } // Foreign key to Reservation
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }
    public DateTime PaymentDate { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Reservation Reservation { get; set; } // One-to-one with Reservation
}
