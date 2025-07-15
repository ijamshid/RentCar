using AutoMapper;
using RentCar.Application.Models.Photo;
using RentCar.Core.Entities;

namespace RentCar.Application.MappingProfiles;

public class PhotoProfile : Profile
{
    public PhotoProfile()
    {
        CreateMap<CreatePhotoModel, Photo>().ReverseMap();

        CreateMap<UpdatePhotoModel, Photo>().ReverseMap();

        CreateMap<Photo, PhotoModel>().ReverseMap();

        CreateMap<CreatePhotoModel, ResponsePhotoModel>().ReverseMap();
    }
}