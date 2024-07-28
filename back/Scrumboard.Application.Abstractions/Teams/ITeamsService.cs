using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Abstractions.Teams;

public interface ITeamsService
{
    Task<Team> GetByIdAsync(TeamId id, CancellationToken cancellationToken = default);
    Task<Team> GetByBoardIdAsync(BoardId boardId, CancellationToken cancellationToken = default);
    Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default);
    Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(TeamId id, CancellationToken cancellationToken = default);
}
