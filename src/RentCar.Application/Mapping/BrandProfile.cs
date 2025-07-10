using AutoMapper;
using RentCar.Application.Models.Brand;
using RentCar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCar.Application.Mapping
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<Brand, BrandGetDto>();
            CreateMap<UpdateBrandDto, Brand>();
        }
    }
}
