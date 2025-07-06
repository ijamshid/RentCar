using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RentCar.Application.Helpers.GenerateJWT;
using SecureLoginApp.Application.Helpers.GenerateJwt;
using System.Text;

namespace RentCar.Application.Helpers
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOption").Get<JwtOption>();

            if (jwtOptions == null)
            {
                throw new InvalidOperationException("JWT sozlamalari topilmadi. appsettings.json faylida 'JwtOption' bo'limini tekshiring.");
            }

           

            return serviceCollection;
        }
    }
}
