﻿using AutoMapper;
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
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<OrderModel, Order>(MemberList.None)
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.SetId, opt => opt.MapFrom(src => src.SetId))
               .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
