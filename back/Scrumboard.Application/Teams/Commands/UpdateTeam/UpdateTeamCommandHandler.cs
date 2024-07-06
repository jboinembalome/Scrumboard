using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

internal sealed class UpdateTeamCommandHandler(
    IMapper mapper,
    ITeamsRepository teamsRepository,
    IIdentityService identityService)
    : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse>
{
    public async Task<UpdateTeamCommandResponse> Handle(
        UpdateTeamCommand request, 
        CancellationToken cancellationToken)
    {
        var updateTeamCommandResponse = new UpdateTeamCommandResponse();
        
        var teamToUpdate = await teamsRepository.TryGetByIdAsync(request.Id , cancellationToken);

        if (teamToUpdate is null)
            throw new NotFoundException(nameof(Comment), request.Id);

        mapper.Map(request, teamToUpdate);

        await teamsRepository.UpdateAsync(teamToUpdate, cancellationToken);

        var userIds = teamToUpdate.Members
            .ToHashSet();
        
        var users = await identityService.GetListAsync(userIds, cancellationToken);
        
        var teamDto = mapper.Map<TeamDto>(teamToUpdate);
        
        mapper.Map(users, teamDto.Adherents);

        updateTeamCommandResponse.Team = teamDto;

        return updateTeamCommandResponse;
    }
}
