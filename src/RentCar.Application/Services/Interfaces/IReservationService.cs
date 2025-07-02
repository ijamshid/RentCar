using RentCar.Application.DTOs;

namespace RentCar.Application.Services.Interfaces
{

    public interface IReservationService
    {
        Task<IEnumerable<ReservationGetDto>> GetAllAsync();
        Task<ReservationGetDto> GetByIdAsync(int id);
        Task<ReservationGetDto> CreateAsync(ReservationCreateDto dto);
        Task<bool> UpdateAsync(ReservationUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
