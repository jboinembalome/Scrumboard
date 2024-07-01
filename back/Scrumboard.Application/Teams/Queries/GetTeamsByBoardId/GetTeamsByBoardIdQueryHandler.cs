using AutoMapper;
using MediatR;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Teams.Queries.GetTeamsByBoardId;

internal sealed class GetTeamsByBoardIdQueryHandler(
    IMapper mapper,
    ITeamsQueryRepository teamsQueryRepository)
    : IRequestHandler<GetTeamsByBoardIdQuery, TeamDto>
{
    public async Task<TeamDto> Handle(
        GetTeamsByBoardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var team = await teamsQueryRepository.TryGetByBoardIdAsync(request.BoardId, cancellationToken);

        return mapper.Map<TeamDto>(team);
    }
}
