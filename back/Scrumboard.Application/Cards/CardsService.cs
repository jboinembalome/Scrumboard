using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.SharedKernel.Exceptions;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Cards;

internal sealed class CardsService(
    IActivitiesRepository activitiesRepository,
    ICardsRepository cardsRepository,
    ICardsQueryRepository cardsQueryRepository,
    ILabelsRepository labelsRepository,
    IIdentityService identityService,
    IValidator<CardCreation> cardCreationValidator,
    IValidator<CardEdition> cardEditionValidator) : ICardsService
{
    public async Task<bool> ExistsAsync(CardId id, CancellationToken cancellationToken = default)
    {
        var card = await cardsRepository.TryGetByIdAsync(id, cancellationToken);

        return card is not null;
    }

    public Task<IReadOnlyList<Card>> GetByListBoardIdAsync(ListBoardId listBoardId, CancellationToken cancellationToken = default)
        => cardsQueryRepository.GetByListBoardIdAsync(listBoardId, cancellationToken);

    public Task<Card> GetByIdAsync(CardId id, CancellationToken cancellationToken = default)
        => cardsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public async Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default)
    {
        await cardCreationValidator.ValidateAndThrowAsync(cardCreation, cancellationToken);
        
        var card = await cardsRepository.AddAsync(cardCreation, cancellationToken);

        var activity = new Activity(card.Id, ActivityType.Added, ActivityField.Card, string.Empty, card.Name);
        await activitiesRepository.AddAsync(activity, cancellationToken);
        
        return card;
    }

    public async Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default)
    {
        var cardToUpdate = await cardsRepository.TryGetByIdAsync(cardEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(cardEdition.Id);
        
        await cardEditionValidator.ValidateAndThrowAsync(cardEdition, cancellationToken);

        // TODO: Update Activities (no more included by cardRepository.TryGetByIdAsync)
        var newActivities = await GetNewActivities(cardToUpdate, cardEdition, cancellationToken);

        if (newActivities.Count > 0)
        {
            await activitiesRepository.AddAsync(newActivities, cancellationToken);
        }
        
        return await cardsRepository.UpdateAsync(cardEdition, cancellationToken);
    }

    public async Task DeleteAsync(CardId id, CancellationToken cancellationToken = default)
    { 
        await cardsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await cardsRepository.DeleteAsync(id, cancellationToken);
    }
    
    // TODO: Move new activities logic into a specific service
    
    /// <summary>
    /// Retrieves the card activies. 
    /// </summary>
    /// <param name="oldCard">Old card.</param>
    /// <param name="cardEdition">Updated card.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private async Task<List<Activity>> GetNewActivities(
        Card oldCard, 
        CardEdition cardEdition, 
        CancellationToken cancellationToken)
    {
        var activities = new List<Activity>();
        
        #region Name
        if (oldCard.Name != cardEdition.Name)
            activities.Add(new Activity(cardEdition.Id, ActivityType.Updated, ActivityField.Name, oldCard.Name, cardEdition.Name));

        #endregion

        #region Description
        if (oldCard.Description != cardEdition.Description)
            activities.Add(new Activity(cardEdition.Id, ActivityType.Updated, ActivityField.Description, oldCard.Description, cardEdition.Description));

        #endregion

        #region DueDate
        if (oldCard.DueDate != cardEdition.DueDate)
        {
            if (!oldCard.DueDate.HasValue && cardEdition.DueDate.HasValue)
                activities.Add(new Activity(cardEdition.Id, ActivityType.Added, ActivityField.DueDate, string.Empty, cardEdition.DueDate.Value.ToShortDateString()));

            if (oldCard.DueDate.HasValue && !cardEdition.DueDate.HasValue)
                activities.Add(new Activity(cardEdition.Id, ActivityType.Removed, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), string.Empty));

            if (oldCard.DueDate.HasValue && cardEdition.DueDate.HasValue && oldCard.DueDate.Value != cardEdition.DueDate.Value)
                activities.Add(new Activity(cardEdition.Id, ActivityType.Updated, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), cardEdition.DueDate.Value.ToShortDateString()));
        }

        #endregion

        #region Member
        if (!oldCard.AssigneeIds.Any() && cardEdition.AssigneeIds.Any())
        {
            var firstAssigneeId = cardEdition.AssigneeIds.First();
            var firstAssignee = await identityService.GetUserAsync(firstAssigneeId, cancellationToken);
            activities.Add(new Activity(cardEdition.Id, ActivityType.Added, ActivityField.Member, string.Empty, $"{firstAssignee.FirstName} {firstAssignee.LastName}"));
        }

        if (oldCard.AssigneeIds.Any() && !cardEdition.AssigneeIds.Any())
        {
            var firstAssigneeId = oldCard.AssigneeIds.First();
            var firstAssignee = await identityService.GetUserAsync(firstAssigneeId, cancellationToken);
            activities.Add(new Activity(cardEdition.Id, ActivityType.Removed, ActivityField.Member, $"{firstAssignee.FirstName} {firstAssignee.LastName}", string.Empty));
        }

        if (oldCard.AssigneeIds.Count < cardEdition.AssigneeIds.Count())
        {
            var firstAssigneeId = cardEdition.AssigneeIds.First(x => !oldCard.AssigneeIds.Contains(x));
            var firstAssignee = await identityService.GetUserAsync(firstAssigneeId, cancellationToken);
            activities.Add(new Activity(cardEdition.Id, ActivityType.Added, ActivityField.Member, string.Empty, $"{firstAssignee.FirstName} {firstAssignee.LastName}"));
        }

        if (oldCard.AssigneeIds.Count > cardEdition.AssigneeIds.Count())
        {
            var firstAssigneeId = oldCard.AssigneeIds.First(l => !cardEdition.AssigneeIds.Select(x => x).Contains(l));
            var firstAssignee = await identityService.GetUserAsync(firstAssigneeId, cancellationToken);
            activities.Add(new Activity(cardEdition.Id, ActivityType.Removed, ActivityField.Member, $"{firstAssignee.FirstName} {firstAssignee.LastName}", string.Empty));
        }
        #endregion
        
        #region Label
        if (!oldCard.LabelIds.Any() && cardEdition.LabelIds.Any())
        {
            var labelId = cardEdition.LabelIds.First();
            var label = await labelsRepository.TryGetByIdAsync(labelId, cancellationToken);
            ArgumentNullException.ThrowIfNull(label);
            
            activities.Add(new Activity(cardEdition.Id, ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
        }

        if (oldCard.LabelIds.Any() && !cardEdition.LabelIds.Any())
        {
            var labelId = oldCard.LabelIds.First();
            var label = await labelsRepository.TryGetByIdAsync(labelId, cancellationToken);
            ArgumentNullException.ThrowIfNull(label);
            
            activities.Add(new Activity(cardEdition.Id, ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
        }

        if (oldCard.LabelIds.Count < cardEdition.LabelIds.Count())
        {
            var labelId = cardEdition.LabelIds.First(l => !oldCard.LabelIds.Select(x => x).Contains(l));
            var label = await labelsRepository.TryGetByIdAsync(labelId, cancellationToken);
            ArgumentNullException.ThrowIfNull(label);
            
            activities.Add(new Activity(cardEdition.Id, ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
        }

        if (oldCard.LabelIds.Count > cardEdition.LabelIds.Count())
        {
            var labelId = oldCard.LabelIds.First(l => !cardEdition.LabelIds.Select(x => x).Contains(l));
            var label = await labelsRepository.TryGetByIdAsync(labelId, cancellationToken);
            ArgumentNullException.ThrowIfNull(label);
            
            activities.Add(new Activity(cardEdition.Id, ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
        }

        if (oldCard.LabelIds.Any() && cardEdition.LabelIds.Any() && oldCard.LabelIds.Count == cardEdition.LabelIds.Count())
        {
            foreach (var oldLabelId in oldCard.LabelIds)
            {
                var updateLabelId = (LabelId?)cardEdition.LabelIds.FirstOrDefault(x => x == oldLabelId);
                if (updateLabelId is null)
                    continue;

                var oldLabel = await labelsRepository.TryGetByIdAsync(oldLabelId, cancellationToken);
                ArgumentNullException.ThrowIfNull(oldLabel);
                
                var updateLabel = await labelsRepository.TryGetByIdAsync(updateLabelId.Value, cancellationToken);
                ArgumentNullException.ThrowIfNull(updateLabel);
                
                if (oldLabelId != updateLabelId)
                    activities.Add(new Activity(cardEdition.Id, ActivityType.Updated, ActivityField.Label, oldLabel.Name, updateLabel.Name));
            }
        }
        #endregion

        return activities;
    }
}
