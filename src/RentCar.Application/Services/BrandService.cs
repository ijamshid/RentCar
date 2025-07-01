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
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public BrandService(DatabaseContext context,
            IMemoryCache cache,
            IMapper mapper,
            MemoryCacheEntryOptions cacheOptions)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
        }
        public async Task Add(CreateBrandDto dto)
        {
            var brand = _mapper.Map<Brand>(dto);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BrandGetDto>> GetAll()
        {
            if (_cache.TryGetValue("brands", out List<BrandGetDto> cachedBrands))
                return cachedBrands;
            var brands = await _context.Brands.ToListAsync();
            var brandDtos = _mapper.Map<List<BrandGetDto>>(brands);

            _cache.Set("brands", brandDtos, _cacheOptions);
            return brandDtos;

        }

        public async Task<BrandGetDto> GetById(int id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with ID {id} not found.");
            }
            var brandDto = _mapper.Map<BrandGetDto>(brand);
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
