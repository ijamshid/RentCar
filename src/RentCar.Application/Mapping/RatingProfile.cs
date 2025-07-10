using AutoMapper;
using RentCar.Application.Models.Rating;
using RentCar.Core.Entities;

namespace RentCar.Application.Mapping
{

    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingGetDto>();
            CreateMap<RatingCreateDto, Rating>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<RatingUpdateDto, Rating>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
