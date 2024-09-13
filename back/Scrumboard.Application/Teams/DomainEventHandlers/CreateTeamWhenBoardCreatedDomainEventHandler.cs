using Scrumboard.Domain.Boards.Events;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.Application.Teams.DomainEventHandlers;

internal sealed class CreateTeamWhenBoardCreatedDomainEventHandler(
    ITeamsRepository teamsRepository) : IDomainEventHandler<BoardCreatedDomainEvent>
{
    public async Task Handle(BoardCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var memberId = new MemberId(domainEvent.OwnerId.Value);
        var team = new Team(
            name: $"Team - {domainEvent.BoardId}",
            boardId: domainEvent.BoardId,
            memberIds: [memberId]);

        await teamsRepository.AddAsync(team, cancellationToken);
    }
}
