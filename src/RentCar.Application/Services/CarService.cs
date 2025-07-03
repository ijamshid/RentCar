using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{
    public class CarService : ICarService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CarService(DatabaseContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task CreateAsync(CarCreateDto dto)
        {
            var car = _mapper.Map<Car>(dto);
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            _cache.Remove("cars"); // invalidate cache
        }

        public void Delete(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
                throw new KeyNotFoundException($"Car with ID {id} not found.");

            _context.Cars.Remove(car);
            _context.SaveChanges();

            _cache.Remove("cars");
            _cache.Remove($"car_{id}");
        }

        public async Task<IEnumerable<CarGetDto>> GetAllAsync()
        {
            const string cacheKey = "cars";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<CarGetDto> cachedCars))
                return cachedCars;

            var cars = await _context.Cars
                .Include(c => c.Brand)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<CarGetDto>>(cars);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<CarGetDto> GetByIdAsync(int id)
        {
            string cacheKey = $"car_{id}";

            if (_cache.TryGetValue(cacheKey, out CarGetDto cachedCar))
                return cachedCar;

            var car = await _context.Cars
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                throw new KeyNotFoundException($"Car with ID {id} not found.");

            var result = _mapper.Map<CarGetDto>(car);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public void Update(CarUpdateDto dto)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == dto.Id);
            if (car == null)
                throw new KeyNotFoundException($"Car with ID {dto.Id} not found.");

            _mapper.Map(dto, car);
            _context.Cars.Update(car);
            _context.SaveChanges();

            _cache.Remove("cars");
            _cache.Remove($"car_{dto.Id}");
        }
    }
}
