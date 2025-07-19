using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.Models.Car;
using RentCar.Application.Models.Reservation;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.Core.Enums;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ReservationService(DatabaseContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<ReservationGetDto>> GetAllAsync()
        {
            const string cacheKey = "reservations";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<ReservationGetDto> cachedReservations))
                return cachedReservations;

            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<ReservationGetDto>>(reservations);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return result;
        }

        public async Task<ReservationGetDto?> GetByIdAsync(int id)
        {
            string cacheKey = $"reservation_{id}";

            if (_cache.TryGetValue(cacheKey, out ReservationGetDto cachedReservation))
                return cachedReservation;

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            var result = _mapper.Map<ReservationGetDto>(reservation);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return result;
        }

        public async Task<ReservationGetDto> CreateReservationAsync(ReservationCreateDto dto, string userId)
        {
            if (!int.TryParse(userId, out int userIdInt))
                throw new UnauthorizedAccessException("Invalid user ID.");

            var car = await _context.Cars.FirstOrDefaultAsync(a => a.Id == dto.CarId);
            if (car == null)
                throw new ArgumentException("Car not found");

            var days = (dto.ReturnDate.Date - dto.PickupDate.Date).Days;
            if (days <= 0)
                throw new ArgumentException("ReturnDate must be after PickupDate");

            var total = days * car.DailyPrice;

            var reservation = new Reservation
            {
                UserId = userIdInt,
                CarId = dto.CarId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                TotalPrice = total,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Payment = new Payment
                {
                    Amount = total,
                    Status = PaymentStatus.Pending,
                    PaymentMethod = PaymentMethod.Cash,
                    TransactionId = Guid.NewGuid().ToString(),
                    PaymentDate = DateTime.UtcNow,
                    UserId=userIdInt
                }
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            _cache.Remove("reservations");

            return _mapper.Map<ReservationGetDto>(reservation);
        }

        public async Task<bool> UpdateAsync(ReservationUpdateDto dto)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(a => a.Id == dto.Id);
            if (reservation == null)
                return false;

            _mapper.Map(dto, reservation);
            await _context.SaveChangesAsync();

            _cache.Remove("reservations");
            _cache.Remove($"reservation_{dto.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(a => a.Id == id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            _cache.Remove("reservations");
            _cache.Remove($"reservation_{id}");

            return true;
        }

        public async Task<ServiceResult<ReservationGetDto>> CancelReservationAsync(int reservationId, string userId)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(a => a.Id == reservationId);

            if (reservation == null)
                return ServiceResult<ReservationGetDto>.Fail("Reservation not found");

            if (reservation.UserId != int.Parse(userId))
                return ServiceResult<ReservationGetDto>.Fail("Unauthorized access");

            if (reservation.Status != ReservationStatus.Pending)
                return ServiceResult<ReservationGetDto>.Fail("Reservation cannot be cancelled");

            reservation.Status = ReservationStatus.Cancelled;
            reservation.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _cache.Remove("reservations");
            _cache.Remove($"reservation_{reservation.Id}");

            var resultDto = _mapper.Map<ReservationGetDto>(reservation);
            return ServiceResult<ReservationGetDto>.Success(resultDto);
        }

        public async Task<ServiceResult<ReservationGetDto>> ReturnCarAsync(int reservationId, ReturnCarDto dto, string userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                return ServiceResult<ReservationGetDto>.Fail("Reservation not found");

            if (reservation.UserId != int.Parse(userId))
                return ServiceResult<ReservationGetDto>.Fail("Unauthorized access");

            if (reservation.Status != ReservationStatus.Confirmed)
                return ServiceResult<ReservationGetDto>.Fail("Reservation cannot be returned");

            var now = DateTime.UtcNow;
            decimal penalty = 0m;

            if (now > reservation.ReturnDate)
            {
                var hoursLate = (now - reservation.ReturnDate).TotalHours;
                penalty = (decimal)(Math.Ceiling(hoursLate) * 5); // penalty 5 per hour late
                reservation.TotalPrice += penalty;
            }

            reservation.Status = ReservationStatus.Completed;
            reservation.LastModifiedAt = now;

            await _context.SaveChangesAsync();

            _cache.Remove("reservations");
            _cache.Remove($"reservation_{reservation.Id}");

            var resultDto = _mapper.Map<ReservationGetDto>(reservation);
            return ServiceResult<ReservationGetDto>.Success(resultDto);
        }
    }
}
