using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Events;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.Cards.Activities;

internal sealed class ActivityFactory(
    IChangeActivityStrategy<DateTimeOffset?> dueDateChangeActivityStrategy,
    IChangeActivityStrategy<ListBoardId> listBoardChangeActivityStrategy,
    IChangeActivityStrategy<IReadOnlyCollection<AssigneeId>> assigneesChangeActivityStrategy,
    IChangeActivityStrategy<IReadOnlyCollection<LabelId>> labelsChangeActivityStrategy) : IActivityFactory
{
    public async Task<IReadOnlyCollection<Activity>> CreateAsync(
        CardUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken) 
        => 
        [
            .. await dueDateChangeActivityStrategy.GetChangeActivitiesAsync(domainEvent.Id, domainEvent.DueDate, cancellationToken),
            .. await listBoardChangeActivityStrategy.GetChangeActivitiesAsync(domainEvent.Id, domainEvent.ListBoardId, cancellationToken),
            .. await assigneesChangeActivityStrategy.GetChangeActivitiesAsync(domainEvent.Id, domainEvent.AssigneeIds, cancellationToken),
            .. await labelsChangeActivityStrategy.GetChangeActivitiesAsync(domainEvent.Id, domainEvent.LabelIds, cancellationToken)
        ];
}
