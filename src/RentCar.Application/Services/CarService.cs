using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RentCar.Application.Common;
using RentCar.Application.Models.Car;
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
        private readonly IFileStorageService _storageService;
        private readonly MinioSettings _minioSettings;

        public CarService(
            DatabaseContext context,
            IMapper mapper,
            IMemoryCache cache,
            IFileStorageService storageService,
            IOptions<MinioSettings> settings)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _storageService = storageService;
            _minioSettings = settings.Value;
        }

        public async Task CreateAsync(CarCreateDto dto)
        {
            string bucket = "car-photos";

            var car = _mapper.Map<Car>(dto);
            car.Photos = new List<CarPhoto>();

            foreach (var formFile in dto.Photos ?? Enumerable.Empty<IFormFile>())
            {
                if (formFile.Length > 0)
                {
                    string objectName = $"{Guid.NewGuid()}_{formFile.FileName}";
                    using var stream = formFile.OpenReadStream();

                    await _storageService.UploadFileAsync(bucket, objectName, stream, formFile.ContentType);

                    car.Photos.Add(new CarPhoto
                    {
                        ObjectName = objectName
                    });
                }
            }

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
                .Include(c => c.Photos)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<CarGetDto>>(cars);
            var resultLits = result.ToList();

            for (int i = 0; i < resultLits.Count(); i++)
            {
                var carEntity = cars[i];
                var dto = resultLits[i];

                dto.ImageUrls = carEntity.Photos
                    .Select(p => $"http://{_minioSettings.Endpoint}/car-photos/{p.ObjectName}")
                    .ToList();
            }

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<IEnumerable<CarGetDto>> GetByBrand(string brand)
        {
            const string cacheKey = "brand_cars";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<CarGetDto> cachedCars))
                return cachedCars;

            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new Exception("Brand name cannot be null or empty.");
            }

            var trimmedName = brand.Trim().ToUpper();

            var cars = await _context.Cars
                .Where(c => EF.Functions.Like(c.Brand.Name.ToUpper(), $"%{trimmedName}%"))
                .ToListAsync();

            if (cars.Count == 0)
            {
                throw new KeyNotFoundException($"No cars found for brand '{brand}'.");
            }


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

            result.ImageUrls = car.Photos
    .Select(p => $"http://{_minioSettings.Endpoint}/car-photos/{p.ObjectName}")
    .ToList();

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
