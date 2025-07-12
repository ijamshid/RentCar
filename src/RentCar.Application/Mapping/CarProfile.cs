using AutoMapper;
using Microsoft.Extensions.Options;
using RentCar.Application.Common;
using RentCar.Application.Models.Brand;
using RentCar.Application.Models.Car;
using RentCar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCar.Application.Mapping
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            Console.WriteLine("CarProfile loaded!");
            CreateMap<CarCreateDto, Car>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
            CreateMap<Car, CarGetDto>()
                .ForMember(dest => dest.ImageGuids,
                           opt => opt.Ignore());
            CreateMap<UpdateBrandDto, Car>();
        }
    }
}
