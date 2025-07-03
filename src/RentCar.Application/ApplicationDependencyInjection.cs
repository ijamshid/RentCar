using Microsoft.Extensions.DependencyInjection;
using RentCar.Application.Services;
using RentCar.Application.Services.Interfaces;

namespace RentCar.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Service registration
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IBrandService, BrandService>();


            // Memory Cache
            services.AddMemoryCache();

            // AutoMapper — agar mapping profillar shu assembly ichida bo‘lsa
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());         

            return services;
        }
    }
}
