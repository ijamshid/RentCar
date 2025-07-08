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

    public class RatingService : IRatingService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public RatingService(DatabaseContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;

           
        }


        public async Task<IEnumerable<RatingGetDto>> GetAllAsync()
        {
            const string cacheKey = "ratings";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RatingGetDto> cachedRatings))
                return cachedRatings;

            var ratings = await _context.Ratings.ToListAsync();
            var result = _mapper.Map<IEnumerable<RatingGetDto>>(ratings);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<RatingGetDto> GetByIdAsync(int id)
        {
            string cacheKey = $"rating_{id}";

            if (_cache.TryGetValue(cacheKey, out RatingGetDto cachedRating))
                return cachedRating;

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
                return null;

            var result = _mapper.Map<RatingGetDto>(rating);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<RatingGetDto> CreateAsync(RatingCreateDto dto)
        {
            var rating = _mapper.Map<Rating>(dto);
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            _cache.Remove("ratings");

            return _mapper.Map<RatingGetDto>(rating);
        }

        public async Task<bool> UpdateAsync(RatingUpdateDto dto)
        {
            var rating = await _context.Ratings.FindAsync(dto.Id);
            if (rating == null)
                return false;

            _mapper.Map(dto, rating);
            await _context.SaveChangesAsync();

            _cache.Remove("ratings");
            _cache.Remove($"rating_{dto.Id}");

            return true;
        }

        public async Task<bool> RateCarAsync(RatingCreateDto dto, int userId)
        {
            // 1️⃣ Reservation mavjudligini tekshirish
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.UserId == userId);

            if (reservation == null)
                return false;

            // 2️⃣ Reservation statusi "Completed" bo‘lishi shart
            if (reservation.Status != ReservationStatus.Completed)
                return false;

            // 3️⃣ Duplicate rating tekshirish (shu foydalanuvchi shu car uchun reyting berganmi)
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.UserId == userId);

            if (existingRating != null)
                return false;

            // 4️⃣ Reytingni DB ga qo‘shish
            var rating = _mapper.Map<Rating>(dto);
            rating.UserId = userId;
            rating.CreatedAt = DateTime.UtcNow;

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            // 5️⃣ Cache tozalash
            _cache.Remove("ratings");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
                return false;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            _cache.Remove("ratings");
            _cache.Remove($"rating_{id}");

            return true;
        }
    }
}
