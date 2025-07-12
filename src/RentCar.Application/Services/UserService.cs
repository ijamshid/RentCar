using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Helpers.PasswordHashers;
using RentCar.Application.Models.User2;
using RentCar.Application.Models.Users;
using RentCar.Application.Services.Impl;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services;


public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IAuthService _authService;

    public UserService(DatabaseContext context, IMapper mapper, IMemoryCache cache, IAuthService authService)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _authService = authService;
    }
    public async Task<IEnumerable<UserGetDto>> GetAllAsync()
    {
        if (_cache.TryGetValue("users", out IEnumerable<UserGetDto> cachedUsers))
            return cachedUsers;

        var users = await _context.Users.ToListAsync();
        var result = _mapper.Map<IEnumerable<UserGetDto>>(users);
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        _cache.Set("users", result, cacheOptions);
        return result;
    }

    public async Task<ApiResult<UserAuthResponseModel>> GetUserAuth()
    {
        if (_authService.User == null)
        {
            return ApiResult<UserAuthResponseModel>.Failure(new List<string> { "User not found" });
        }

        UserAuthResponseModel userPermissions = new UserAuthResponseModel
        {
            Id = _authService.User.Id,
            FullName = _authService.User.FullName,
            Permissions = _authService.User.Permissions
        };

        return ApiResult<UserAuthResponseModel>.Success(userPermissions);
    }
    public async Task<UserGetDto> GetByIdAsync(int id)
    {
        string cacheKey = $"user_{id}";
        if (_cache.TryGetValue(cacheKey, out UserGetDto cachedUser))
            return cachedUser;

        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (user == null) return null;

        var result = _mapper.Map<UserGetDto>(user);
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        _cache.Set(cacheKey, result, cacheOptions);
        return result;
    }

    public async Task<UserGetDto> CreateAsync(UserCreateDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var hash = new PasswordHasher();
        var salt=hash.GenerateSalt();
        user.PasswordHash = hash.Encrypt(dto.Password, salt);
        user.IsActive = true;
        user.Salt= salt;
        string roleName = dto.IsAdmin ? "Admin" : "User";

        _context.Users.Add(user);
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = defaultRole.Id
        });
        await _context.SaveChangesAsync();

        _cache.Remove("users"); // cache invalidation

        return _mapper.Map<UserGetDto>(user);
    }

    public async Task<bool> UpdateAsync(UserUpdateDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == dto.Id);
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
        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        _cache.Remove("users");
        _cache.Remove($"user_{id}");

        return true;
    }


}