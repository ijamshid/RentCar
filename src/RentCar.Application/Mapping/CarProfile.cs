using AutoMapper;
using RentCar.Application.DTOs;
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
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<Brand, BrandGetDto>();
            CreateMap<UpdateBrandDto, Brand>();
        }
    }
}
