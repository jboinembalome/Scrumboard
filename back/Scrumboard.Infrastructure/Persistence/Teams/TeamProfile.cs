using AutoMapper;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<Team, TeamDao>();
        
        // Read
        CreateMap<TeamDao, Team>();
    }
}
