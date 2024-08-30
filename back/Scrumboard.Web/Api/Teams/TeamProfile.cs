using AutoMapper;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Web.Api.Users;

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
        
        CreateMap<TeamMember, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MemberId));
    }
}
