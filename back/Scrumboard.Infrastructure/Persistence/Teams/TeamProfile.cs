using AutoMapper;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<TeamCreation, Team>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds));
        CreateMap<TeamEdition, Team>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds));
    }
}
