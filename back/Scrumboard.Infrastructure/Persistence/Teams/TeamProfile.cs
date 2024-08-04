using AutoMapper;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<TeamCreation, TeamDao>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds));
        CreateMap<TeamEdition, TeamDao>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds));
        
        CreateMap<UserId, TeamMemberDao>()
            .ConstructUsing(memberId => new TeamMemberDao
            {
                MemberId = memberId
            });
        
        // Read
        CreateMap<TeamDao, Team>()
            .ForMember(dest => dest.MemberIds, opt => opt.MapFrom(src => src.Members));
        
        CreateMap<TeamMemberDao, UserId>()
            .ConstructUsing(teamMemberDao =>  (UserId)teamMemberDao.MemberId);
    }
}
