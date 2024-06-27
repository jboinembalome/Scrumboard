using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Application.Teams.Specifications;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

internal sealed class UpdateTeamCommandHandler(
    IMapper mapper,
    IAsyncRepository<Team, int> teamRepository,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<UpdateTeamCommandResponse> Handle(
        UpdateTeamCommand request, 
        CancellationToken cancellationToken)
    {
        var updateTeamCommandResponse = new UpdateTeamCommandResponse();

        var specification = new TeamWithAdherentsSpec(request.Id);
        var teamToUpdate = await teamRepository.FirstOrDefaultAsync(specification , cancellationToken);

        if (teamToUpdate == null)
            throw new NotFoundException(nameof(Comment), request.Id);

        mapper.Map(request, teamToUpdate);

        await teamRepository.UpdateAsync(teamToUpdate, cancellationToken);

        var users = await identityService.GetListAsync(teamToUpdate.Adherents, cancellationToken);
        //var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(teamToUpdate.Adherents);


        var teamDto = mapper.Map<TeamDto>(teamToUpdate);
        //teamDto.Adherents = _mapper.Map(users, adherentDtos);

        mapper.Map(users, teamDto.Adherents);

        updateTeamCommandResponse.Team = teamDto;

        return updateTeamCommandResponse;

        //return Unit.Value;
    }
}
