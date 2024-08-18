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
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds))
            .ForMember(dest => dest.Members, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.AddMembers(src.MemberIds));
        CreateMap<TeamEdition, Team>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds))
            .ForMember(dest => dest.Members, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.AddMembers(src.MemberIds));
    }
}

internal static class TeamExtensions
{
    public static void AddMembers(this Team team, IReadOnlyCollection<MemberId> memberIds)
    {
        foreach (var memberId in memberIds)
        {
            team.AddMember(memberId);
        }
    }
}


