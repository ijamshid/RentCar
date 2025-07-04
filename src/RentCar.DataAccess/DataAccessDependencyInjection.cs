using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RentCar.DataAccess.Persistence;
using Microsoft.Extensions.Configuration;

namespace RentCar.DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(connectionString, opt =>
            {
                opt.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName);
                opt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); // Retry policy qo‘shildi
            })
            .UseSnakeCaseNamingConvention()
        );
    }
}
