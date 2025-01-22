using AutoMapper;
using ContactManagement.Application.DTOs;
using ContactManagement.Application.Users.DTOs;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContactManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber.Value));

            CreateMap<Address, AddressDto>();

            CreateMap<User, UserDto>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles.Select(r => r.Role)));
        }
    }
}
