using RentCar.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCar.Application.Services.Interfaces
{

    public interface IUserService
    {
        Task<IEnumerable<UserGetDto>> GetAllAsync();
        Task<UserGetDto> GetByIdAsync(int id);
        Task<UserGetDto> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
