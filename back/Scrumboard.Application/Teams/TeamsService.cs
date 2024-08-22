using AutoMapper;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Teams;

internal sealed class TeamsService(
    IMapper mapper,
    ITeamsRepository teamsRepository,
    ITeamsQueryRepository teamsQueryRepository) : ITeamsService
{
    public Task<Team> GetByIdAsync(
        TeamId id, 
        CancellationToken cancellationToken = default) 
        => teamsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
    
    public async Task<Team> GetByBoardIdAsync(
        BoardId boardId, 
        CancellationToken cancellationToken = default) 
        => await teamsQueryRepository.TryGetByBoardIdAsync(boardId, cancellationToken)
            .OrThrowResourceNotFoundAsync(boardId);

    // TODO: Add validation
    public Task<Team> AddAsync(
        TeamCreation teamCreation, 
        CancellationToken cancellationToken = default)
    {
        var team = mapper.Map<Team>(teamCreation);
        
        return teamsRepository.AddAsync(team, cancellationToken);
    }

    // TODO: Add validation
    public async Task<Team> UpdateAsync(
        TeamEdition teamEdition, 
        CancellationToken cancellationToken = default)
    {
        var team = await teamsRepository.TryGetByIdAsync(teamEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(teamEdition.Id);

        mapper.Map(teamEdition, team);

        teamsRepository.Update(team);
            
        return team;
    }

    public async Task DeleteAsync(
        TeamId id, 
        CancellationToken cancellationToken = default)
    {
        await teamsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await teamsRepository.DeleteAsync(id, cancellationToken);
    }
}
