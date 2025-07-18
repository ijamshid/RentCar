﻿using AutoMapper;
using RentCar.Application.Models.Car;
using RentCar.Application.Models.User2;
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

        }
    }
}
