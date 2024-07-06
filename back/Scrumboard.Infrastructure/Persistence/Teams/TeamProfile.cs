using AutoMapper;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<Team, TeamDao>();
        
        CreateMap<string, TeamMemberDao>()
            .ConstructUsing(memberId => new TeamMemberDao
            {
                MemberId = memberId
            });
        
        // Read
        CreateMap<TeamDao, Team>();
        
        CreateMap<TeamMemberDao, string>()
            .ConstructUsing(teamMemberDao =>  teamMemberDao.MemberId);
    }
}
