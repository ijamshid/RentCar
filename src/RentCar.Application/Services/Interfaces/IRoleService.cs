using RentCar.Application.Models.Roles;

namespace RentCar.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(int id);
        Task CreateAsync(string name);
        Task UpdateAsync(int id, string newName);
        Task DeleteAsync(int id); // optional
    }

}
