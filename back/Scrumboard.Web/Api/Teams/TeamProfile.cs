using AutoMapper;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;

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
