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
            .ForMember(dest => dest.Members, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.AddMembers(src.MemberIds));
        CreateMap<TeamEdition, Team>()
            .ForMember(dest => dest.Members, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.UpdateMembers(src.MemberIds));
    }
}


