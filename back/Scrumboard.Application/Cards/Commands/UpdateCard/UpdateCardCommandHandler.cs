using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Application.Cards.Commands.UpdateCard;

internal sealed class UpdateCardCommandHandler(
    IMapper mapper,
    IActivitiesRepository activitiesRepository,
    ICardsRepository cardsRepository,
    IIdentityService identityService)
    : IRequestHandler<UpdateCardCommand, UpdateCardCommandResponse>
{
    public async Task<UpdateCardCommandResponse> Handle(
        UpdateCardCommand request, 
        CancellationToken cancellationToken)
    {
        var updateCardCommandResponse = new UpdateCardCommandResponse();
        
        var cardToUpdate = await cardsRepository.TryGetByIdAsync(request.Id, cancellationToken);

        if (cardToUpdate is null)
            throw new NotFoundException(nameof(Card), request.Id);
        
        // TODO: Update Activities (no more included by cardRepository.TryGetByIdAsync)
        var newActivities = await GetNewActivities(cardToUpdate, request, cancellationToken);

        if (newActivities.Count > 0)
        {
            await activitiesRepository.AddAsync(newActivities, cancellationToken);
        }
          
        mapper.Map(request, cardToUpdate, typeof(UpdateCardCommand), typeof(Card));

        await cardsRepository.UpdateAsync(cardToUpdate, cancellationToken);

        updateCardCommandResponse.Card = mapper.Map<CardDetailDto>(cardToUpdate);

        if (updateCardCommandResponse.Card.Assignees.Any())
        {
            var assigneeIds = cardToUpdate.Assignees;
            var users = await identityService.GetListAsync(assigneeIds, cancellationToken);
            
            mapper.Map(users, updateCardCommandResponse.Card.Assignees);
        }


        return updateCardCommandResponse;
    }
    
    // TODO: Move new activities logic into a specific service
    
    /// <summary>
    /// Retrieves the card activies. 
    /// </summary>
    /// <param name="oldCard">Old card.</param>
    /// <param name="updatedCard">Updated card.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private async Task<List<Activity>> GetNewActivities(Card oldCard, UpdateCardCommand updatedCard, CancellationToken cancellationToken)
    {
        var activities = new List<Activity>();
        #region Name
        if (oldCard.Name != updatedCard.Name)
            activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.Name, oldCard.Name, updatedCard.Name));

        #endregion

        #region Description
        if (oldCard.Description != updatedCard.Description)
            activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.Description, oldCard.Description, updatedCard.Description));

        #endregion

        #region DueDate
        if (oldCard.DueDate != updatedCard.DueDate)
        {
            if (!oldCard.DueDate.HasValue && updatedCard.DueDate.HasValue)
                activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.DueDate, string.Empty, updatedCard.DueDate.Value.ToShortDateString()));

            if (oldCard.DueDate.HasValue && !updatedCard.DueDate.HasValue)
                activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), string.Empty));

            if (oldCard.DueDate.HasValue && updatedCard.DueDate.HasValue && oldCard.DueDate.Value != updatedCard.DueDate.Value)
                activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), updatedCard.DueDate.Value.ToShortDateString()));
        }

        #endregion

        #region Member
        if (!oldCard.Assignees.Any() && updatedCard.Assignees.Any())
        {
            var adherent = updatedCard.Assignees.First();
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Member, string.Empty, $"{adherent.FirstName} {adherent.LastName}"));
        }

        if (oldCard.Assignees.Any() && !updatedCard.Assignees.Any())
        {
            var adherent = oldCard.Assignees.First();
            var user = await identityService.GetUserAsync(adherent, cancellationToken);
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Member, $"{user.FirstName} {user.LastName}", string.Empty));
        }

        if (oldCard.Assignees.Count < updatedCard.Assignees.Count())
        {
            var adherent = updatedCard.Assignees.First(l => !oldCard.Assignees.Contains(l.Id));
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Member, string.Empty, $"{adherent.FirstName} {adherent.LastName}"));
        }

        if (oldCard.Assignees.Count > updatedCard.Assignees.Count())
        {
            var adherent = oldCard.Assignees.First(l => !updatedCard.Assignees.Select(o => o.Id).Contains(l));
            var user = await identityService.GetUserAsync(adherent, cancellationToken);
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Member, $"{user.FirstName} {user.LastName}", string.Empty));
        }
        #endregion

        #region Checklist
        if (!oldCard.Checklists.Any() && updatedCard.Checklists.Any())
        {
            var checklist = updatedCard.Checklists.First();
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Checklist, string.Empty, checklist.Name));
        }

        if (oldCard.Checklists.Any() && !updatedCard.Checklists.Any())
        {
            var checklist = oldCard.Checklists.First();
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Checklist, checklist.Name, string.Empty));
        }

        if (oldCard.Checklists.Count < updatedCard.Checklists.Count())
        {
            var checklist = updatedCard.Checklists.First(l => !oldCard.Checklists.Select(o => o.Id).Contains(l.Id));
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Checklist, string.Empty, checklist.Name));
        }

        if (oldCard.Checklists.Count > updatedCard.Checklists.Count())
        {
            var checklist = oldCard.Checklists.First(l => !updatedCard.Checklists.Select(o => o.Id).Contains(l.Id));
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Checklist, checklist.Name, string.Empty));
        }

        if (oldCard.Checklists.Any() && updatedCard.Checklists.Any() && oldCard.Checklists.Count == updatedCard.Checklists.Count())
        {
            foreach (var oldChecklist in oldCard.Checklists)
            {
                var updateChecklist = updatedCard.Checklists.FirstOrDefault(c => c.Id == oldChecklist.Id);
                if (updateChecklist == null)
                    continue;

                if (oldChecklist.Name != updateChecklist.Name)
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.Checklist, oldChecklist.Name, updateChecklist.Name));

                #region ChecklistItem
                if (!oldChecklist.ChecklistItems.Any() && updateChecklist.ChecklistItems.Any())
                {
                    var checklistItem = updateChecklist.ChecklistItems.First();
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.ChecklistItem, string.Empty, checklistItem.Name));
                }

                if (oldChecklist.ChecklistItems.Any() && !updateChecklist.ChecklistItems.Any())
                {
                    var checklistItem = oldChecklist.ChecklistItems.First();
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.ChecklistItem, checklistItem.Name, string.Empty));
                }

                if (oldChecklist.ChecklistItems.Count < updateChecklist.ChecklistItems.Count())
                {
                    var checklistItem = updateChecklist.ChecklistItems.First(l => !oldChecklist.ChecklistItems.Select(o => o.Id).Contains(l.Id));
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.ChecklistItem, string.Empty, checklistItem.Name));
                }

                if (oldChecklist.ChecklistItems.Count > updateChecklist.ChecklistItems.Count())
                {
                    var checklistItem = oldChecklist.ChecklistItems.First(l => !updateChecklist.ChecklistItems.Select(o => o.Id).Contains(l.Id));
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.ChecklistItem, checklistItem.Name, string.Empty));
                }

                if (oldChecklist.ChecklistItems.Any() && updateChecklist.ChecklistItems.Any() && oldChecklist.ChecklistItems.Count == updateChecklist.ChecklistItems.Count())
                {
                    foreach (var oldChecklistItem in oldChecklist.ChecklistItems)
                    {
                        var updateChecklistItem = updateChecklist.ChecklistItems.FirstOrDefault(c => c.Id == oldChecklistItem.Id);
                        if (updateChecklistItem == null)
                            continue;

                        if (oldChecklistItem.Name != updateChecklistItem.Name)
                            activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.ChecklistItem, oldChecklistItem.Name, updateChecklistItem.Name));

                        if (!oldChecklistItem.IsChecked && updateChecklistItem.IsChecked)
                            activities.Add(new Activity(updatedCard.Id, ActivityType.Checked, ActivityField.ChecklistItem, string.Empty, $"{updateChecklistItem.Name} in {updateChecklist.Name}"));

                        if (oldChecklistItem.IsChecked && !updateChecklistItem.IsChecked)
                            activities.Add(new Activity(updatedCard.Id, ActivityType.Unchecked, ActivityField.ChecklistItem, string.Empty, $"{updateChecklistItem.Name} in {updateChecklist.Name}"));
                    }

                    if (!oldChecklist.ChecklistItems.All(c => c.IsChecked) && updateChecklist.ChecklistItems.All(c => c.IsChecked))
                        activities.Add(new Activity(updatedCard.Id, ActivityType.Finished, ActivityField.Checklist, string.Empty, oldChecklist.Name));

                    if (oldChecklist.ChecklistItems.All(c => c.IsChecked) && !updateChecklist.ChecklistItems.All(c => c.IsChecked))
                        activities.Add(new Activity(updatedCard.Id, ActivityType.NotFinished, ActivityField.Checklist, string.Empty, oldChecklist.Name));

                }
                #endregion
            }
        }
        #endregion

        #region Label
        if (!oldCard.Labels.Any() && updatedCard.Labels.Any())
        {
            var label = updatedCard.Labels.First();
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
        }

        if (oldCard.Labels.Any() && !updatedCard.Labels.Any())
        {
            var label = oldCard.Labels.First();
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
        }

        if (oldCard.Labels.Count < updatedCard.Labels.Count())
        {
            var label = updatedCard.Labels.First(l => !oldCard.Labels.Select(o => o.Id).Contains(l.Id));
            activities.Add(new Activity(updatedCard.Id, ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
        }

        if (oldCard.Labels.Count > updatedCard.Labels.Count())
        {
            var label = oldCard.Labels.First(l => !updatedCard.Labels.Select(o => o.Id).Contains(l.Id));
            activities.Add(new Activity(updatedCard.Id, ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
        }

        if (oldCard.Labels.Any() && updatedCard.Labels.Any() && oldCard.Labels.Count == updatedCard.Labels.Count())
        {
            foreach (var oldLabel in oldCard.Labels)
            {
                var updateLabel = updatedCard.Labels.FirstOrDefault(c => c.Id == oldLabel.Id);
                if (updateLabel == null)
                    continue;

                if (oldLabel.Name != updateLabel.Name)
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.Label, oldLabel.Name, updateLabel.Name));

                if (oldLabel.Colour.Code != updateLabel.Colour.Colour)
                    activities.Add(new Activity(updatedCard.Id, ActivityType.Updated, ActivityField.Label, oldLabel.Colour.Code, updateLabel.Colour.Colour));
            }
        }
        #endregion

        return activities;
    }

}
