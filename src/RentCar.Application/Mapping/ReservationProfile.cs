using AutoMapper;
using RentCar.Application.DTOs;
using RentCar.Core.Entities;

namespace RentCar.Application.Mapping
{

    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationGetDto>();
            CreateMap<ReservationCreateDto, Reservation>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore());

            CreateMap<ReservationUpdateDto, Reservation>()
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
