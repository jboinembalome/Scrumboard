using AutoMapper;
using Scrumboard.Application.Teams.Commands.UpdateTeam;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Application.Teams;

internal sealed class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<UpdateTeamCommand, Team>();
        CreateMap<TeamDto, Team>();
        
        // Read
        CreateMap<Team, TeamDto>();
    }
}
