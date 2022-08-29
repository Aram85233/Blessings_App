using AutoMapper;
using Blessings.Data.Entities;
using Blessings.Models;

namespace Blessings.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, SignUpModel>()
        .ForMember(dest =>
            dest.Email,
            opt => opt.MapFrom(src => src.Email))
        .ForMember(dest =>
            dest.FullName,
            opt => opt.MapFrom(src => src.FullName))
        .ForMember(dest =>
            dest.Password,
            opt => opt.MapFrom(src => src.Password));
        }
        
    }
}
