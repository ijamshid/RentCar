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
        public CarProfile(IOptions<MinioSettings> settings)
        {
            var minioEndpoint = settings.Value.Endpoint;
            var bucket = "car-photos";

            CreateMap<CarCreateDto, Car>();
            CreateMap<Car, CarGetDto>()
                .ForMember(dest => dest.ImageUrls,
                           opt => opt.MapFrom(src =>
                           src.Photos.Select(p => $"http://{minioEndpoint}/{bucket}/{p.ObjectName}")
                           ));
            CreateMap<UpdateBrandDto, Car>();
        }
    }
}
