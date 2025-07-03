using RentCar.Core.Entities;

namespace RentCar.Application.Helpers.GenerateJWT
{
    public interface IJwtTokenHandler
    {
        string GenerateAccessToken(User user, string sessionToken);
        string GenerateRefreshToken();
    }
}
