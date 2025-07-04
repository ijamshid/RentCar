using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Helpers.PasswordHashers;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;
using System.IdentityModel.Tokens.Jwt;

namespace RentCar.Application.Services;


public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly IOtpService _otpService;

    public UserService(DatabaseContext context, IMapper mapper, IMemoryCache cache, IJwtTokenHandler jwtTokenHandler,
        IPasswordHasher hasher, IEmailService email, IOtpService otp)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _jwtTokenHandler = jwtTokenHandler;
        _passwordHasher = hasher;
        _emailService = email;
        _otpService = otp;
    }


    //Registration and Login
    public async Task<ApiResult<string>> RegisterAsync(string fullname, string email, string password, bool isAdminSite)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
            return ApiResult<string>.Failure(new[] { "Email allaqachon mavjud" });

        var salt = Guid.NewGuid().ToString();
        var hash = _passwordHasher.Encrypt(password, salt);

        var nameParts = fullname.Split(' ', 2);
        string firstname = nameParts[0];
        string lastname = nameParts.Length > 1 ? nameParts[1] : "";

        var user = new User
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            PasswordHash = hash,
            Salt = salt,
            IsActive = false,
            CreatedAt = DateTime.Now,
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var roleName = isAdminSite ? "Admin" : "User";
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

        if (role == null)
        {
            return ApiResult<string>.Failure(new[] { $"Tizimda '{roleName}' roli topilmadi. Admin bilan bog'laning." });
        }

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });
        await _context.SaveChangesAsync();

        var otp = await _otpService.GenerateOtpAsync(user.Email);
        await _emailService.SendOtpAsync(user.Email, otp);

        return ApiResult<string>.Success("Ro'yxatdan o'tdingiz. Email orqali tasdiqlang.");
    }

    public async Task<ApiResult<AuthResponse>> VerifyOtpAsync(string email, string otpCode)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return ApiResult<AuthResponse>.Failure(new[] { "Foydalanuvchi topilmadi" });

        if (!_otpService.ValidateOtp(email, otpCode))
            return ApiResult<AuthResponse>.Failure(new[] { "Noto‘g‘ri yoki muddati o‘tgan kod" });

        user.IsActive = true;
        await _context.SaveChangesAsync();

        var token = _jwtTokenHandler.GenerateAccessToken(user, Guid.NewGuid().ToString());
        var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync();

        var response = new AuthResponse
        {
            Email = user.Email,
            Fullname = $"{user.Firstname} {user.Lastname}",
            Token = token,
            RefreshToken = refreshToken
        };

        return ApiResult<AuthResponse>.Success(response);
    }

    public async Task<ApiResult<AuthResponse>> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !user.IsActive)
            return ApiResult<AuthResponse>.Failure(new[] { "Email yoki parol noto‘g‘ri" });

        var isCorrect = _passwordHasher.Verify(password, user.Salt, user.PasswordHash);
        if (!isCorrect)
            return ApiResult<AuthResponse>.Failure(new[] { "Email yoki parol noto‘g‘ri" });

        var token = _jwtTokenHandler.GenerateAccessToken(user, Guid.NewGuid().ToString());
        var refreshToken = _jwtTokenHandler.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync();

        var response = new AuthResponse
        {
            Email = user.Email,
            Fullname = $"{user.Firstname} {user.Lastname}",
            Token = token,
            RefreshToken = refreshToken
        };

        return ApiResult<AuthResponse>.Success(response);
    }

    //User CRUD

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


    public async Task<UserGetDto> GetByIdAsync(int id)
    {
        string cacheKey = $"user_{id}";
        if (_cache.TryGetValue(cacheKey, out UserGetDto cachedUser))
            return cachedUser;

        var user = await _context.Users.FindAsync(id);
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

        // Password hashing (masalan, BCrypt ishlatish mumkin)
        user.PasswordHash = "s"; //BCrypt.Net.BCrypt.HashPassword(dto.Password);
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