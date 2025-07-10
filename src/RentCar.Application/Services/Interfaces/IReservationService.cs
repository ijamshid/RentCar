using RentCar.Application.Models.Car;
using RentCar.Application.Models.Reservation;

namespace RentCar.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationGetDto>> GetAllAsync();
        Task<ReservationGetDto?> GetByIdAsync(int id);

        Task<ReservationGetDto> CreateReservationAsync(ReservationCreateDto dto, string userId);

        Task<bool> UpdateAsync(ReservationUpdateDto dto);

        Task<bool> DeleteAsync(int id);

        // Maxsus reservation statuslarini boshqarish metodlari

        Task<ServiceResult<ReservationGetDto>> CancelReservationAsync(int reservationId, string userId);

        Task<ServiceResult<ReservationGetDto>> ReturnCarAsync(int reservationId, ReturnCarDto dto, string userId);
    }

    // Qo‘shimcha yordamchi sinf (agar kerak bo‘lsa)
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static ServiceResult<T> Fail(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }
}
