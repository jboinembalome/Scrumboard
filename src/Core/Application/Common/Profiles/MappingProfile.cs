using AutoMapper;
using Scrumboard.Application.Common.Dtos;
using Scrumboard.Domain.Common;

namespace Scrumboard.Application.Common.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Read
        CreateMap<Colour, ColourDto>()
            .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Code));
    }
}