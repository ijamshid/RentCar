using RentCar.Application.DTOs;

namespace RentCar.Application.Services.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarGetDto>> GetAllAsync();
        Task<CarGetDto> GetByIdAsync(int id);
        Task CreateAsync(CarCreateDto dto);
        void Update(CarUpdateDto dto);
        void Delete(int id);
    }
}
