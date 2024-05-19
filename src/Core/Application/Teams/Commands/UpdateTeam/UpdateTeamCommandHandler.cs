using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Application.Teams.Specifications;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Entities;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

internal sealed class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse>
{
    private readonly IAsyncRepository<Team, int> _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(
        IMapper mapper, 
        IAsyncRepository<Team, int> teamRepository, 
        ICurrentUserService currentUserService, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<UpdateTeamCommandResponse> Handle(
        UpdateTeamCommand request, 
        CancellationToken cancellationToken)
    {
        var updateTeamCommandResponse = new UpdateTeamCommandResponse();

        var specification = new TeamWithAdherentsSpec(request.Id);
        var teamToUpdate = await _teamRepository.FirstOrDefaultAsync(specification , cancellationToken);

        if (teamToUpdate == null)
            throw new NotFoundException(nameof(Comment), request.Id);

        _mapper.Map(request, teamToUpdate);

        await _teamRepository.UpdateAsync(teamToUpdate, cancellationToken);

        var users = await _identityService.GetListAsync(teamToUpdate.Adherents.Select(a => a.IdentityId), cancellationToken);
        //var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(teamToUpdate.Adherents);


        var teamDto = _mapper.Map<TeamDto>(teamToUpdate);
        //teamDto.Adherents = _mapper.Map(users, adherentDtos);

        _mapper.Map(users, teamDto.Adherents);

        updateTeamCommandResponse.Team = teamDto;

        return updateTeamCommandResponse;

        //return Unit.Value;
    }
}