using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents;

internal sealed class AdherentProfile : Profile
{
    public AdherentProfile()
    {
        CreateMap<AdherentDto, Adherent>()
            .EqualityComparison((dest, src) => dest.Id == src.Id);
        
        // Read
        CreateMap<Adherent, AdherentDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);

        CreateMap<IUser, AdherentDto>()
            .EqualityComparison((dest, src) => dest.Id == src.IdentityId)
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdentityId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.HasAvatar, opt => opt.MapFrom(src => src.Avatar.Any()));
    }
}
