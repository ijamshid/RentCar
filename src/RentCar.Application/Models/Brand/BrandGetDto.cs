namespace RentCar.Application.Models.Brand
{
    public class BrandGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional: agar har bir brandga tegishli mashinalar sonini ko‘rsatmoqchi bo‘lsangiz
        public int CarCount { get; set; }
    }
}
