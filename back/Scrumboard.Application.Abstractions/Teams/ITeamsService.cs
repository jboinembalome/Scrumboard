using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Abstractions.Teams;

public interface ITeamsService
{
    Task<Team> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Team> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default);
    Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
