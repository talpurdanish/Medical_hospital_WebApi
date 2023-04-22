using AutoMapper;
using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Profiles
{
    public class CityProfile : Profile
    {

        public CityProfile()
        {
            CreateMap<City, CityViewModel>();
        }
    }
}
