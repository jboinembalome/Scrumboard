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
        var memberId = new MemberId(domainEvent.OwnerId);
        var teamCreation = new TeamCreation
        {
            Name = $"Team - {domainEvent.BoardId}",
            MemberIds = [memberId],
            BoardId = domainEvent.BoardId
        };

        await teamsRepository.AddAsync(teamCreation, cancellationToken);
    }
}
