using Domain.Viewmodels;
using Domain.Models;
using AutoMapper;

namespace FMDC.Profiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<PatientViewModel, Patient>();
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.Created, act => act.MapFrom(src => GetCreatedDate(src.PatientId, src.Created)));
        }
        private static DateTime GetCreatedDate(int UserId, DateTime Created)
        {
            return UserId == -1 ? DateTime.Now : Created;
        }
    }
}
