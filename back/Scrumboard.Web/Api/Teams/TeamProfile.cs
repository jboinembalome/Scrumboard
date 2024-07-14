using AutoMapper;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Web.Api.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<TeamCreationDto, TeamCreation>();
        CreateMap<TeamEditionDto, TeamEdition>();
        
        // Read
        CreateMap<Team, TeamDto>();
    }
}
