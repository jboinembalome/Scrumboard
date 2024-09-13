using AutoMapper;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Application.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<TeamCreation, Team>()
            .ConstructUsing(src => new Team(src.Name, src.BoardId, src.MemberIds));
        CreateMap<TeamEdition, Team>()
            .ForMember(dest => dest.Members, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.UpdateMembers(src.MemberIds));
    }
}


