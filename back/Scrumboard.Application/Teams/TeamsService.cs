using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Teams;

internal sealed class TeamsService(
    ITeamsRepository teamsRepository,
    ITeamsQueryRepository teamsQueryRepository) : ITeamsService
{
    public async Task<Team> GetByIdAsync(TeamId id, CancellationToken cancellationToken = default) 
        => await teamsQueryRepository.TryGetByIdAsync(id, cancellationToken) 
           ?? throw new NotFoundException(nameof(Team), id);
    
    public async Task<Team> GetByBoardIdAsync(BoardId boardId, CancellationToken cancellationToken = default) 
        => await teamsQueryRepository.TryGetByBoardIdAsync(boardId, cancellationToken) 
           ?? throw new NotFoundException(nameof(Team), boardId);

    // TODO: Add validation
    public Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default)
        => teamsRepository.AddAsync(teamCreation, cancellationToken);

    // TODO: Add validation
    public Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default) 
        => teamsRepository.UpdateAsync(teamEdition, cancellationToken);

    public async Task DeleteAsync(TeamId id, CancellationToken cancellationToken = default)
    {
        _ = await teamsRepository.TryGetByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Team), id);
        
        await teamsRepository.DeleteAsync(id, cancellationToken);
    }
}
