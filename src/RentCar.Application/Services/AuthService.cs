using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Helpers.PasswordHasher;
using RentCar.Application.Models.Users;
using RentCar.Application.Security;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;
using System.Security.Claims;

namespace RentCar.Application.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DatabaseContext _context;
    private readonly IEmailService _emailService;
    private readonly IOtpService _otpService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private bool? _isAuthenticated; // Bu field IsAuthenticated property'si uchun kesh bo'ladi.
    private HashSet<string>? _permissions; // Bu field Permissions property'si uchun kesh bo'ladi.
    private IUser? _user; // Bu field User property'si uchun kesh bo'ladi.

    public AuthService(IHttpContextAccessor httpContextAccessor, DatabaseContext context,
        IEmailService emailservice, IOtpService otpService, IPasswordHasher passwordHasher,
        IJwtTokenHandler jwtTokenHandler)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _emailService = emailservice;
        _otpService = otpService;
        _passwordHasher = passwordHasher;
        _jwtTokenHandler = jwtTokenHandler;
    }

    public async Task<ApiResult<string>> RegisterAsync(RegisterUserModel model)
    {
        var existingUser = await _context.Users.AnyAsync(e => e.Email == model.Email);
        if (existingUser)
            return ApiResult<string>.Failure(new[] { "Email allaqachon mavjud" });

        var salt = Guid.NewGuid().ToString();
        var hash = _passwordHasher.Encrypt(model.Password, salt);

        var user = new User
        {
            Firstname = model.FirstName,
            Lastname = model.LastName,
            Email = model.Email,
            PasswordHash = hash,
            DateOfBirth = model.DateOfBirth,
            PhoneNumber = model.PhoneNumber,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            IsVerified = false // Yangi foydalanuvchilar odatda tasdiqlanmagan holda boshlanadi
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // --- Rolni isAdminSite ga qarab belgilash ---
        string roleName =  "User";
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

        var otp = await _otpService.GenerateOtpAsync(user.Email);

        return ApiResult<string>.Success("Ro'yxatdan o'tdingiz. Emailingizni tasdiqlang.");
    }

    public async Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model)
    {
        var user = await _context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
        .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user is null)
            return ApiResult<LoginResponseModel>.Failure(new[] { "Foydalanuvchi topilmadi" });

        if (!_passwordHasher.Verify(user.PasswordHash, model.Password, user.Salt))
            return ApiResult<LoginResponseModel>.Failure(new[] { "Parol yoki email noto‘g‘ri" });

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
            Roles = user.UserRoles?
                       .Where(ur => ur.Role != null && !string.IsNullOrEmpty(ur.Role.Name))
                       .Select(ur => ur.Role.Name)
                       .ToList() ?? new List<string>(),

            Permissions = user.UserRoles?
                            .Where(ur => ur.Role?.RolePermissions != null)
                            .SelectMany(ur => ur.Role.RolePermissions)
                            .Where(rp => rp.Permission != null && !string.IsNullOrEmpty(rp.Permission.ShortName))
                            .Select(rp => rp.Permission.ShortName)
                            .Distinct()
                            .ToList() ?? new List<string>()
        });

    }

    public async Task<bool> VerifyOtpAsync(string email, string inputCode)
    {
        var otp = await _context.UserOTPs
            .Where(o => o.Email == email && o.Code == inputCode && o.ExpiredAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otp == null)
            return false;  // Kod noto‘g‘ri yoki muddati o‘tgan

        // Kod to‘g‘ri, foydalanuvchini tasdiqlashni amalga oshirish mumkin
        var user = await _context.Users.FirstOrDefaultAsync(a=>a.Email==email);
        if (user == null) return false;

        user.IsVerified = true;
        await _context.SaveChangesAsync();

        return true;
    }

 
    // --- Muhim o'zgarishlar bu yerda ---
    public bool IsAuthenticated
    {
        get
        {
            // Agar _isAuthenticated allaqachon hisoblangan bo'lsa, uni qaytar.
            if (_isAuthenticated == null)
            {
                var httpContext = _httpContextAccessor.HttpContext;

                // Avval HTTP kontekstida foydalanuvchi autentifikatsiya qilinganmi, tekshiramiz.
                // Bu joyda User property'sini chaqirmaymiz, faqat Identity ni tekshiramiz.
                bool baseAuthenticated = httpContext?.User?.Identity?.IsAuthenticated ?? false;

                if (baseAuthenticated)
                {
                    // Agar bazaviy autentifikatsiya muvaffaqiyatli bo'lsa,
                    // endi User property'sini chaqirib, uning IsVerified holatini tekshiramiz.
                    // User property'si esa o'z ichida _user keshini boshqaradi va bazadan ma'lumotni yuklaydi.
                    var currentUser = User;

                    // Faqat autentifikatsiya qilingan va tasdiqlangan bo'lsa haqiqiy deb hisoblaymiz.
                    _isAuthenticated = (currentUser != null && currentUser.IsVerified);
                }
                else
                {
                    _isAuthenticated = false;
                }
            }
            return _isAuthenticated.GetValueOrDefault();
        }
    }

    public IUser User
    {
        get
        {
            if (_user != null)
            {
                return _user;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    _user = _context.Set<User>().AsQueryable()
                        .Where(a => a.Id == userId)
                        .Select(a => new UserAuthModel
                        {
                            Id = a.Id,
                            FullName = a.Firstname + " " + a.Lastname,
                            IsVerified = a.IsVerified,
                            Permissions = a.UserRoles
                                .SelectMany(ur => ur.Role.RolePermissions)
                                .Select(rp => rp.Permission.ShortName)
                                .Distinct()
                                .ToHashSet(),
                        })
                        .FirstOrDefault();
                }
            }

            return _user;
        }
    }


    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(a => a.Type == CustomClaimNames.Id);

        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            Console.WriteLine("⚠️ User ID not found in claims.");
            return 0;
        }
        return int.Parse(userIdClaim.Value);
    }

    public HashSet<string> Permissions
    {
        get
        {
            if (_permissions == null)
            {
                var currentUser = User; // User property'sini chaqiramiz. Bu _user ni yuklaydi.
                _permissions = currentUser?.Permissions?.ToHashSet() ?? new HashSet<string>();
            }
            return _permissions;
        }
    }

    public bool HasPermission(params string[] permissionCodes) =>
         permissionCodes.All(a => Permissions.Contains(a));
}