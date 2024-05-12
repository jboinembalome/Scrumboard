using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Adherents.Specifications;
using Scrumboard.Application.Features.Cards.Specifications;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.Enums;
using Scrumboard.Domain.ValueObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Features.Cards.Commands.UpdateCard
{
    public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, UpdateCardCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Card, int> _cardRepository;
        private readonly IAsyncRepository<Label, int> _labelRepository;
        private readonly IAsyncRepository<Board, int> _boardRepository;
        private readonly IAsyncRepository<Adherent, int> _adherentRepository;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCardCommandHandler(
            IMapper mapper,
            IAsyncRepository<Card, int> cardRepository,
            IAsyncRepository<Label, int> labelRepository,
            IAsyncRepository<Board, int> boardRepository,
            IAsyncRepository<Adherent, int> adherentRepository,
            IIdentityService identityService,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _labelRepository = labelRepository;
            _boardRepository = boardRepository;
            _adherentRepository = adherentRepository;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateCardCommandResponse> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var updateCardCommandResponse = new UpdateCardCommandResponse();

            var specification = new CardWithAllExceptComment(request.Id);
            var cardToUpdate = await _cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (cardToUpdate == null)
                throw new NotFoundException(nameof(Card), request.Id);

            var adherentSpecification = new AdherentByUserIdSpec(_currentUserService.UserId);
            var adherent = await _adherentRepository.FirstOrDefaultAsync(adherentSpecification, cancellationToken);
            var newActivities = await GetNewActivities(cardToUpdate, request, cancellationToken);

            newActivities.ForEach(a =>
            {
                a.Adherent = adherent;
                if (cardToUpdate.Activities.Any())
                    cardToUpdate.Activities.Add(a);
            });

            if (!cardToUpdate.Activities.Any())
                cardToUpdate.Activities = new Collection<Activity>(newActivities);
          
            _mapper.Map(request, cardToUpdate, typeof(UpdateCardCommand), typeof(Card));

            await _cardRepository.UpdateAsync(cardToUpdate, cancellationToken);

            updateCardCommandResponse.Card = _mapper.Map<CardDetailDto>(cardToUpdate);

            if (updateCardCommandResponse.Card.Adherents.Any())
            {
                var users = await _identityService.GetListAsync(cardToUpdate.Adherents.Select(a => a.IdentityId), cancellationToken);
                _mapper.Map(users, updateCardCommandResponse.Card.Adherents);
            }

            if (updateCardCommandResponse.Card.Comments.Any())
            {
                var users = await _identityService.GetListAsync(cardToUpdate.Comments.Select(c => c.Adherent.IdentityId), cancellationToken);
                var adherentDtos = updateCardCommandResponse.Card.Comments.Select(c => c.Adherent).ToList();

                MapUsers(users, adherentDtos);
            }

            if (updateCardCommandResponse.Card.Activities.Any())
            {
                var users = await _identityService.GetListAsync(cardToUpdate.Activities.Select(c => c.Adherent.IdentityId), cancellationToken);
                var adherentDtos = updateCardCommandResponse.Card.Activities.Select(c => c.Adherent).ToList();

                MapUsers(users, adherentDtos);
            }


            return updateCardCommandResponse;
        }

        public void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
        {
            foreach (var adherent in adherents)
            {
                var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
                if (user == null)
                    continue;

                _mapper.Map(user, adherent);
            }
        }

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
                activities.Add(new Activity(ActivityType.Updated, ActivityField.Name, oldCard.Name, updatedCard.Name));

            #endregion

            #region Description
            if (oldCard.Description != updatedCard.Description)
                activities.Add(new Activity(ActivityType.Updated, ActivityField.Description, oldCard.Description, updatedCard.Description));

            #endregion

            #region DueDate
            if (oldCard.DueDate != updatedCard.DueDate)
            {
                if (!oldCard.DueDate.HasValue && updatedCard.DueDate.HasValue)
                    activities.Add(new Activity(ActivityType.Added, ActivityField.DueDate, string.Empty, updatedCard.DueDate.Value.ToShortDateString()));

                if (oldCard.DueDate.HasValue && !updatedCard.DueDate.HasValue)
                    activities.Add(new Activity(ActivityType.Removed, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), string.Empty));

                if (oldCard.DueDate.HasValue && updatedCard.DueDate.HasValue && oldCard.DueDate.Value != updatedCard.DueDate.Value)
                    activities.Add(new Activity(ActivityType.Updated, ActivityField.DueDate, oldCard.DueDate.Value.ToShortDateString(), updatedCard.DueDate.Value.ToShortDateString()));
            }

            #endregion

            #region Member
            if (!oldCard.Adherents.Any() && updatedCard.Adherents.Any())
            {
                var adherent = updatedCard.Adherents.First();
                activities.Add(new Activity(ActivityType.Added, ActivityField.Member, string.Empty, $"{adherent.FirstName} {adherent.LastName}"));
            }

            if (oldCard.Adherents.Any() && !updatedCard.Adherents.Any())
            {
                var adherent = oldCard.Adherents.First();
                var user = await _identityService.GetUserAsync(adherent.IdentityId, cancellationToken);
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Member, $"{user.FirstName} {user.LastName}", string.Empty));
            }

            if (oldCard.Adherents.Count < updatedCard.Adherents.Count())
            {
                var adherent = updatedCard.Adherents.First(l => !oldCard.Adherents.Select(o => o.Id).Contains(l.Id));
                activities.Add(new Activity(ActivityType.Added, ActivityField.Member, string.Empty, $"{adherent.FirstName} {adherent.LastName}"));
            }

            if (oldCard.Adherents.Count > updatedCard.Adherents.Count())
            {
                var adherent = oldCard.Adherents.First(l => !updatedCard.Adherents.Select(o => o.Id).Contains(l.Id));
                var user = await _identityService.GetUserAsync(adherent.IdentityId, cancellationToken);
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Member, $"{user.FirstName} {user.LastName}", string.Empty));
            }
            #endregion

            #region Checklist
            if (!oldCard.Checklists.Any() && updatedCard.Checklists.Any())
            {
                var checklist = updatedCard.Checklists.First();
                activities.Add(new Activity(ActivityType.Added, ActivityField.Checklist, string.Empty, checklist.Name));
            }

            if (oldCard.Checklists.Any() && !updatedCard.Checklists.Any())
            {
                var checklist = oldCard.Checklists.First();
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Checklist, checklist.Name, string.Empty));
            }

            if (oldCard.Checklists.Count < updatedCard.Checklists.Count())
            {
                var checklist = updatedCard.Checklists.First(l => !oldCard.Checklists.Select(o => o.Id).Contains(l.Id));
                activities.Add(new Activity(ActivityType.Added, ActivityField.Checklist, string.Empty, checklist.Name));
            }

            if (oldCard.Checklists.Count > updatedCard.Checklists.Count())
            {
                var checklist = oldCard.Checklists.First(l => !updatedCard.Checklists.Select(o => o.Id).Contains(l.Id));
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Checklist, checklist.Name, string.Empty));
            }

            if (oldCard.Checklists.Any() && updatedCard.Checklists.Any() && oldCard.Checklists.Count == updatedCard.Checklists.Count())
            {
                foreach (var oldChecklist in oldCard.Checklists)
                {
                    var updateChecklist = updatedCard.Checklists.FirstOrDefault(c => c.Id == oldChecklist.Id);
                    if (updateChecklist == null)
                        continue;

                    if (oldChecklist.Name != updateChecklist.Name)
                        activities.Add(new Activity(ActivityType.Updated, ActivityField.Checklist, oldChecklist.Name, updateChecklist.Name));

                    #region ChecklistItem
                    if (!oldChecklist.ChecklistItems.Any() && updateChecklist.ChecklistItems.Any())
                    {
                        var checklistItem = updateChecklist.ChecklistItems.First();
                        activities.Add(new Activity(ActivityType.Added, ActivityField.ChecklistItem, string.Empty, checklistItem.Name));
                    }

                    if (oldChecklist.ChecklistItems.Any() && !updateChecklist.ChecklistItems.Any())
                    {
                        var checklistItem = oldChecklist.ChecklistItems.First();
                        activities.Add(new Activity(ActivityType.Removed, ActivityField.ChecklistItem, checklistItem.Name, string.Empty));
                    }

                    if (oldChecklist.ChecklistItems.Count < updateChecklist.ChecklistItems.Count())
                    {
                        var checklistItem = updateChecklist.ChecklistItems.First(l => !oldChecklist.ChecklistItems.Select(o => o.Id).Contains(l.Id));
                        activities.Add(new Activity(ActivityType.Added, ActivityField.ChecklistItem, string.Empty, checklistItem.Name));
                    }

                    if (oldChecklist.ChecklistItems.Count > updateChecklist.ChecklistItems.Count())
                    {
                        var checklistItem = oldChecklist.ChecklistItems.First(l => !updateChecklist.ChecklistItems.Select(o => o.Id).Contains(l.Id));
                        activities.Add(new Activity(ActivityType.Removed, ActivityField.ChecklistItem, checklistItem.Name, string.Empty));
                    }

                    if (oldChecklist.ChecklistItems.Any() && updateChecklist.ChecklistItems.Any() && oldChecklist.ChecklistItems.Count == updateChecklist.ChecklistItems.Count())
                    {
                        foreach (var oldChecklistItem in oldChecklist.ChecklistItems)
                        {
                            var updateChecklistItem = updateChecklist.ChecklistItems.FirstOrDefault(c => c.Id == oldChecklistItem.Id);
                            if (updateChecklistItem == null)
                                continue;

                            if (oldChecklistItem.Name != updateChecklistItem.Name)
                                activities.Add(new Activity(ActivityType.Updated, ActivityField.ChecklistItem, oldChecklistItem.Name, updateChecklistItem.Name));

                            if (!oldChecklistItem.IsChecked && updateChecklistItem.IsChecked)
                                activities.Add(new Activity(ActivityType.Checked, ActivityField.ChecklistItem, string.Empty, $"{updateChecklistItem.Name} in {updateChecklist.Name}"));

                            if (oldChecklistItem.IsChecked && !updateChecklistItem.IsChecked)
                                activities.Add(new Activity(ActivityType.Unchecked, ActivityField.ChecklistItem, string.Empty, $"{updateChecklistItem.Name} in {updateChecklist.Name}"));
                        }

                        if (!oldChecklist.ChecklistItems.All(c => c.IsChecked) && updateChecklist.ChecklistItems.All(c => c.IsChecked))
                            activities.Add(new Activity(ActivityType.Finished, ActivityField.Checklist, string.Empty, oldChecklist.Name));

                        if (oldChecklist.ChecklistItems.All(c => c.IsChecked) && !updateChecklist.ChecklistItems.All(c => c.IsChecked))
                            activities.Add(new Activity(ActivityType.NotFinished, ActivityField.Checklist, string.Empty, oldChecklist.Name));

                    }
                    #endregion
                }
            }
            #endregion

            #region Label
            if (!oldCard.Labels.Any() && updatedCard.Labels.Any())
            {
                var label = updatedCard.Labels.First();
                activities.Add(new Activity(ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
            }

            if (oldCard.Labels.Any() && !updatedCard.Labels.Any())
            {
                var label = oldCard.Labels.First();
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
            }

            if (oldCard.Labels.Count < updatedCard.Labels.Count())
            {
                var label = updatedCard.Labels.First(l => !oldCard.Labels.Select(o => o.Id).Contains(l.Id));
                activities.Add(new Activity(ActivityType.Added, ActivityField.Label, string.Empty, label.Name));
            }

            if (oldCard.Labels.Count > updatedCard.Labels.Count())
            {
                var label = oldCard.Labels.First(l => !updatedCard.Labels.Select(o => o.Id).Contains(l.Id));
                activities.Add(new Activity(ActivityType.Removed, ActivityField.Label, label.Name, string.Empty));
            }

            if (oldCard.Labels.Any() && updatedCard.Labels.Any() && oldCard.Labels.Count == updatedCard.Labels.Count())
            {
                foreach (var oldLabel in oldCard.Labels)
                {
                    var updateLabel = updatedCard.Labels.FirstOrDefault(c => c.Id == oldLabel.Id);
                    if (updateLabel == null)
                        continue;

                    if (oldLabel.Name != updateLabel.Name)
                        activities.Add(new Activity(ActivityType.Updated, ActivityField.Label, oldLabel.Name, updateLabel.Name));

                    if (oldLabel.Colour.Code != updateLabel.Colour.Colour)
                        activities.Add(new Activity(ActivityType.Updated, ActivityField.Label, oldLabel.Colour.Code, updateLabel.Colour.Colour));
                }
            }
            #endregion

            return activities;
        }

    }
}
