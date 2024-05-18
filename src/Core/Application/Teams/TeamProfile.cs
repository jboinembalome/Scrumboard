using AutoMapper;
using Scrumboard.Application.Teams.Commands.UpdateTeam;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Teams;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<UpdateTeamCommand, Team>();
    }
}