using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
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

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<ReservationGetDto> GetByIdAsync(int id)
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

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<ReservationGetDto> CreateAsync(ReservationCreateDto dto)
        {
            var car = await _context.Cars.FindAsync(dto.CarId);
            var days = (dto.ReturnDate - dto.PickupDate).Days;
            var total = days * car.DailyPrice;

            var reservation = _mapper.Map<Reservation>(dto);

            reservation.TotalPrice = total;
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.Status = ReservationStatus.Pending;
            reservation.Payment = new Payment
            {
                Amount = total,
                Status = PaymentStatus.Pending,
                PaymentMethod = PaymentMethod.Cash, // Default payment method
                TransactionId = Guid.NewGuid().ToString(), // Generate a unique transaction ID
                PaymentDate = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Cache invalidate (GetAll cache ni tozalaymiz)
            _cache.Remove("reservations");

            var result = _mapper.Map<ReservationGetDto>(reservation);

            return result;
        }

        public async Task<bool> ConfirmReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null || reservation.Status != ReservationStatus.Pending)
                return false;

            reservation.Status = ReservationStatus.Confirmed;
            reservation.LastModifiedAt = DateTime.UtcNow;
            reservation.Payment.Status = PaymentStatus.Completed;
            reservation.Payment.PaymentDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);

            if (reservation == null || reservation.Status != ReservationStatus.Pending)
                return false;

            reservation.Status = ReservationStatus.Cancelled;
            reservation.LastModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null || reservation.Status != ReservationStatus.Confirmed)
                return false;

            var now = DateTime.UtcNow;
            if (now > reservation.ReturnDate)
            {
                var hoursLate = (now - reservation.ReturnDate).TotalHours;
                var penalty = Math.Ceiling(hoursLate) * 5; // 5 per hour
                reservation.TotalPrice += (decimal)penalty;
                reservation.Status = ReservationStatus.Completed;
            }
            else
            {
                reservation.Status = ReservationStatus.Completed;
            }

            reservation.LastModifiedAt = now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ReservationUpdateDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.Id);
            if (reservation == null)
                return false;

            _mapper.Map(dto, reservation);
            await _context.SaveChangesAsync();

            // Cache invalidate
            _cache.Remove("reservations");
            _cache.Remove($"reservation_{dto.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            // Cache invalidate
            _cache.Remove("reservations");
            _cache.Remove($"reservation_{id}");

            return true;
        }
    }
}
