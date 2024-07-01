using AutoMapper;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents;

internal sealed class AdherentProfile : Profile
{
    public AdherentProfile()
    {
        // Read
        CreateMap<Adherent, AdherentDto>();
        
        CreateMap<IUser, AdherentDto>()
            .ForMember(dest => dest.HasAvatar, opt => opt.MapFrom(src => src.Avatar.Any()));
        
        // Write
        CreateMap<AdherentDto, Adherent>();
    }
}
