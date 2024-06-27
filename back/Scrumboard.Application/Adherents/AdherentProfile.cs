using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents;

internal sealed class AdherentProfile : Profile
{
    public AdherentProfile()
    {
        // Read
        CreateMap<IUser, AdherentDto>()
            .EqualityComparison((dest, src) => dest.Id == src.Id)
            .ForMember(dest => dest.HasAvatar, opt => opt.MapFrom(src => src.Avatar.Any()));
    }
}
