using Microsoft.Extensions.DependencyInjection;
using Minio;
using Rentcar.Application.Services.Implementation;
using RentCar.Application.Common;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Helpers.PasswordHasher;
using RentCar.Application.Helpers.PasswordHashers;
using RentCar.Application.Services;
using RentCar.Application.Services.Impl;
using RentCar.Application.Services.Interfaces;
using SecureLoginApp.Application.Helpers.GenerateJwt;
using SecureLoginApp.Application.Services.Impl;

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
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPhotoService, PhotoService>();
            //services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IMinioClient, MinioClient>();
            services.AddScoped<MinioSettings>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddHostedService<RabbitMQConsumer>();
            services.AddHttpContextAccessor();
            




            // Memory Cache
            services.AddMemoryCache();

            // AutoMapper — agar mapping profillar shu assembly ichida bo‘lsa
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
