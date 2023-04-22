using AutoMapper;
using Domain.Viewmodels;
using Domain.Models;
using FMDC.Security;

namespace FMDC.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Created, act => act.MapFrom(src => GetCreatedDate(src.id, src.Created)))
                 .ForMember(dest => dest.IsActive, act => act.MapFrom(src => GetStatus(src.id, src.IsActive)))
                 .ForMember(dest => dest.UserId, act => act.Ignore())
                 .ForMember(dest => dest.Picture, act => act.Ignore())
                 .ForMember(dest => dest.Password, act => act.Ignore())
                 .ForMember(dest => dest.PasswordSalt, act => act.Ignore()); ;
            CreateMap<User, UserViewModel>();

        }

        private static DateTime GetCreatedDate(int UserId, DateTime Created)
        {
            return UserId == -1 ? DateTime.Now : Created;
        }

        private static bool GetStatus(int UserId, bool? Status)
        {
            return (bool)(UserId == -1 ? true : Status != null ? Status : false);
        }




    }
}
