using AutoMapper;
using CourtApp.Api.DTOs;
using CourtApp.Api.Models;

namespace CourtApp.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ContactRequest, ContactRequestDto>();
        CreateMap<CreateContactRequestDto, ContactRequest>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<UpdateContactRequestDto, ContactRequest>();
    }
}