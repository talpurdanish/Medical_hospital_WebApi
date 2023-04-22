using AutoMapper;
using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Profiles
{
    public class RecieptProfile : Profile
    {

        public RecieptProfile()
        {
            CreateMap<RecieptViewModel, Reciept>()
                .ForMember(dest => dest.RecieptDate, act => act.MapFrom(src =>
                DateTime.Parse(src.Date, System.Globalization.CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.RecieptDiscount, act => act.MapFrom(src => src.Discount))
                .ForMember(dest => dest.RecieptTime, act => act.MapFrom(src => src.Time));
            CreateMap<Reciept, RecieptViewModel>();
        }
    }
}
