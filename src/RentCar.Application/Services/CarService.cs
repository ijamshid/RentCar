using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{
    public class CarService(DatabaseContext context, IMapper mapper) : ICarService
    {
        public async Task CreateAsync(CarCreateDto dto)
        {
            var car = mapper.Map<Car>(dto);
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var car = context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found.");
            }
            context.Cars.Remove(car);
            context.SaveChanges();
        }

        public async Task<IEnumerable<CarGetDto>> GetAllAsync()
        {
            var cars = await context.Cars.ToListAsync();
            return mapper.Map<IEnumerable<CarGetDto>>(cars);
        }

        public async Task<CarGetDto> GetByIdAsync(int id)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found.");
            }
            return mapper.Map<CarGetDto>(car);
        }

        public void Update(CarUpdateDto dto)
        {
            var car = context.Cars.FirstOrDefault(c => c.Id == dto.Id);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {dto.Id} not found.");
            }
            context.Cars.Update(mapper.Map<Car>(car));
            context.SaveChanges();
        }
    }
}
