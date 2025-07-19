namespace RentCar.Application.Services;

public interface IDataSeedService
{
    Task SeedRolesAndPermissionsAsync();
}