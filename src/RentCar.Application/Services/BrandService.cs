using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly DatabaseContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        
        public BrandService(DatabaseContext context,
            IMemoryCache cache,
            IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task Add(CreateBrandDto dto)
        {
            var brand = _mapper.Map<Brand>(dto);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            _cache.Remove("brands");
        }

        public async Task<List<BrandGetDto>> GetAllAsync()
        {
            if ( _cache.TryGetValue("brands", out List<BrandGetDto> cachedBrands))
                return cachedBrands;
            var brands = await _context.Brands.ToListAsync();
            var brandDtos = _mapper.Map<List<BrandGetDto>>(brands);
            var cacheOptions = new MemoryCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _cache.Set("brands", brandDtos, cacheOptions);
            return brandDtos;

        }

        public async Task<BrandGetDto> GetById(int id)
        {
            string cacheKey = $"brands_{id}";

            if (_cache.TryGetValue(cacheKey, out BrandGetDto cachedBrand))
                return cachedBrand;

            var brand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found.");

            var brandDto = _mapper.Map<BrandGetDto>(brand);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, brandDto, cacheOptions);

            return brandDto;
        }

        public void Update(UpdateBrandDto dto)
        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == dto.Id);
            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with ID {dto.Id} not found.");
            }
            _mapper.Map(dto, brand);
            _context.Brands.Update(brand);
            _context.SaveChanges();
            _cache.Remove("brands");
            _cache.Remove($"brands_{dto.Id}");
        }

        public void Delete(int id)
        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with ID {id} not found.");
            }
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            _cache.Remove("brands");
            _cache.Remove($"brands_{id}");
            
        }

        
    }
}
