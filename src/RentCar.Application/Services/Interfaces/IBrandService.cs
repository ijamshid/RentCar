using RentCar.Application.DTOs;

namespace RentCar.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task Add(CreateBrandDto dto);
        Task<List<BrandGetDto>> GetAllAsync();
        Task<BrandGetDto> GetById(int id);
        void Update(UpdateBrandDto dto);
        void Delete(int id);
    }
}
