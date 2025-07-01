using AutoMapper;
using RentCar.Application.DTOs;
using RentCar.Core.Entities;

namespace RentCar.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserGetDto>();
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // password hash controllerda qilamiz
            CreateMap<UserUpdateDto, User>();

            CreateMap<CarCreateDto, Car>();
            CreateMap<Car, CarGetDto>();
            CreateMap<CarUpdateDto, Car>();
        }
    }
}
