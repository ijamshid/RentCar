using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.DTOs;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Helpers.PasswordHashers;
using RentCar.Application.Models.Users;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

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
    private readonly IAuthService _authService;


    public UserService(DatabaseContext context, IMapper mapper, IMemoryCache cache, IJwtTokenHandler jwtTokenHandler,
        IPasswordHasher hasher, IEmailService email, IOtpService otp, IAuthService auth)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _jwtTokenHandler = jwtTokenHandler;
        _passwordHasher = hasher;
        _emailService = email;
        _otpService = otp;
        _authService = auth;
    }

    public async Task<ApiResult<string>> RegisterAsync(string firstname,string lastname, string email, string password, bool isAdminSite)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
            return ApiResult<string>.Failure(new[] { "Email allaqachon mavjud" });

        var salt = Guid.NewGuid().ToString();
        var hash = _passwordHasher.Encrypt(password, salt);

        var user = new User
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            PasswordHash = hash,
            Salt = salt,
            CreatedAt = DateTime.Now,
            IsVerified = false // Yangi foydalanuvchilar odatda tasdiqlanmagan holda boshlanadi
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // --- Rolni isAdminSite ga qarab belgilash ---
        string roleName = isAdminSite ? "Admin" : "User";
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

        if (defaultRole == null)
        {
            // Agar kerakli rol topilmasa, xato qaytaramiz
            return ApiResult<string>.Failure(new[] { $"Tizimda '{roleName}' roli topilmadi. Admin bilan bog'laning." });
        }

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = defaultRole.Id
        });
        await _context.SaveChangesAsync();
        // --- Rolni belgilash qismi tugadi ---

        var otp = await _otpService.GenerateOtpAsync(user.Id);
        await _emailService.SendOtpAsync(email, otp);

        return ApiResult<string>.Success("Ro'yxatdan o'tdingiz. Email orqali tasdiqlang.");
    }

    public async Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user is null)
            return ApiResult<LoginResponseModel>.Failure(new[] { "Foydalanuvchi topilmadi" });

        if (!_passwordHasher.Verify(user.PasswordHash, model.Password, user.Salt))
            return ApiResult<LoginResponseModel>.Failure(new[] { "Parol noto‘g‘ri" });

        if (!user.IsVerified)
            return ApiResult<LoginResponseModel>.Failure(new[] { "Email tasdiqlanmagan" });

        var accessToken = _jwtTokenHandler.GenerateAccessToken(user, Guid.NewGuid().ToString());
        var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

        return ApiResult<LoginResponseModel>.Success(new LoginResponseModel
        {
            Email = user.Email,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
            Permissions = user.UserRoles
                 .SelectMany(ur => ur.Role.RolePermissions)
                 .Select(p => p.Permission.ShortName)
                 .Distinct()
                 .ToList()
        });
    }

    public async Task<ApiResult<string>> VerifyOtpAsync(OtpVerificationModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user is null)
            return ApiResult<string>.Failure(new[] { "Foydalanuvchi topilmadi." });

        var otp = await _otpService.GetLatestOtpAsync(user.Id, model.Code);
        if (otp is null || otp.ExpiredAt < DateTime.Now)
            return ApiResult<string>.Failure(new[] { "Kod noto‘g‘ri yoki muddati tugagan." });

        user.IsVerified = true;
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("OTP muvaffaqiyatli tasdiqlandi.");
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