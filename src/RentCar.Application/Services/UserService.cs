using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services;


public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public UserService(DatabaseContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // 5 daqiqa cache
    }

    public async Task<IEnumerable<UserGetDto>> GetAllAsync()
    {
        if (_cache.TryGetValue("users", out IEnumerable<UserGetDto> cachedUsers))
            return cachedUsers;

        var users = await _context.Users.ToListAsync();
        var result = _mapper.Map<IEnumerable<UserGetDto>>(users);

        _cache.Set("users", result, _cacheOptions);
        return result;
    }
  

    public async Task<UserGetDto> GetByIdAsync(int id)
    {
        string cacheKey = $"user_{id}";
        if (_cache.TryGetValue(cacheKey, out UserGetDto cachedUser))
            return cachedUser;

        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        var result = _mapper.Map<UserGetDto>(user);
        _cache.Set(cacheKey, result, _cacheOptions);
        return result;
    }

    public async Task<UserGetDto> CreateAsync(UserCreateDto dto)
    {
        var user = _mapper.Map<User>(dto);

        // Password hashing (masalan, BCrypt ishlatish mumkin)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.IsActive = true;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _cache.Remove("users"); // cache invalidation

        return _mapper.Map<UserGetDto>(user);
    }

    public async Task<bool> UpdateAsync(UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(dto.Id);
        if (user == null)
            return false;

        _mapper.Map(dto, user);
        await _context.SaveChangesAsync();

        _cache.Remove("users");
        _cache.Remove($"user_{dto.Id}");

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        _cache.Remove("users");
        _cache.Remove($"user_{id}");

        return true;
    }
}