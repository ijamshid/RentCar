using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
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
            var reservation = _mapper.Map<Reservation>(dto);
            reservation.PickupDate = reservation.PickupDate.ToUniversalTime();
            reservation.ReturnDate = reservation.ReturnDate.ToUniversalTime();
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Cache invalidate (GetAll cache ni tozalaymiz)
            _cache.Remove("reservations");

            var result = _mapper.Map<ReservationGetDto>(reservation);

            return result;
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
