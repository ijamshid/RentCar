namespace RentCar.Application.DTOs
{
    public class UpdateBrandDto
    {
        public int Id { get; set; } // Qaysi brand yangilanayotganini aniqlash uchun
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
    }
}
