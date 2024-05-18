using AutoMapper;
using Scrumboard.Application.Teams.Commands.UpdateTeam;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Teams;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        // Write
        CreateMap<UpdateTeamCommand, Team>();
        
        // Read
        CreateMap<Team, TeamDto>();
    }
}