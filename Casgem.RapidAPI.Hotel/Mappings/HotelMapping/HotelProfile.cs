using AutoMapper;
using Casgem.RapidAPI.Hotel.Entities;
using static Casgem.RapidAPI.Hotel.Entities.HotelEntity;

namespace Casgem.RapidAPI.Hotel.Mappings.HotelMapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Entity, HotelDetailInfo>().ReverseMap();
        }
    }
}
