using RentCar.Application.Models.Rating;

namespace RentCar.Application.Services.Interfaces
{

    public interface IRatingService
    {
        Task<IEnumerable<RatingGetDto>> GetAllAsync();
        Task<RatingGetDto> GetByIdAsync(int id);
        Task<RatingGetDto> CreateAsync(RatingCreateDto dto, string id);
        Task<bool> UpdateAsync(RatingUpdateDto dto);
        Task<bool> DeleteAsync(int id);

    }
}
