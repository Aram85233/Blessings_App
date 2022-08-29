using AutoMapper;
using Blessings.Data.Entities;
using Blessings.Models;

namespace Blessings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpModel, User>(MemberList.None)
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        }
    }
}
