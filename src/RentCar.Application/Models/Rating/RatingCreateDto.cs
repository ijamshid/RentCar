using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.Rating
{
    public class RatingCreateDto
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
    }


}
