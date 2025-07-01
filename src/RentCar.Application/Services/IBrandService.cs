using RentCar.Application.DTOs;

namespace RentCar.Application.Services
{
    public interface IBrandService
    {
        Task Add(CreateBrandDto dto);
        Task<List<BrandGetDto>> GetAll();
        Task<BrandGetDto> GetById(int id);
        void Update(UpdateBrandDto dto);
        void Delete(int id);
    }
}
