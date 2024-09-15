using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Application.Cards;

internal abstract class CardInputBaseValidator<TInput> 
    : AbstractValidator<TInput> where TInput : CardInputBase
{
    private readonly IListBoardsRepository _listBoardsRepository;
    private readonly ILabelsRepository _labelsRepository;
    private readonly IIdentityService _identityService;

    protected CardInputBaseValidator(
        IListBoardsRepository listBoardsRepository,
        ILabelsRepository labelsRepository,
        IIdentityService identityService)
    {
        _listBoardsRepository = listBoardsRepository;
        _labelsRepository = labelsRepository;
        _identityService = identityService;

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(p => p.Position)
            .GreaterThan(0);

        RuleFor(x => x.ListBoardId)
            .MustAsync(ListBoardExistsAsync)
            .WithMessage("{PropertyName} not found.");
        
        RuleFor(x => x.LabelIds)
            .NotEmpty()
            .MustAsync(LabelsExistAsync)
                .WithMessage("{PropertyName} has not found values: {LabelIdsNotFound}.");
        
        RuleFor(x => x.AssigneeIds)
            .NotEmpty()
            .MustAsync(AssigneesExistAsync)
            .WithMessage("{PropertyName} has not found values: {AssigneeIdsNotFound}.");
    }
    
    private async Task<bool> ListBoardExistsAsync(ListBoardId listBoardId, CancellationToken cancellationToken)
    {
        var listBoard = await _listBoardsRepository.TryGetByIdAsync(listBoardId, cancellationToken);

        return listBoard is not null;
    }
    
    private async Task<bool> LabelsExistAsync(
        TInput cardInput, 
        IEnumerable<LabelId> labelIds,
        ValidationContext<TInput> validationContext,
        CancellationToken cancellationToken)
    {
        var ids = labelIds.ToArray();
        
        var labels = await _labelsRepository.GetAsync(ids, cancellationToken);

        var idsNotFound = ids
            .Except(labels.Select(x => x.Id))
            .ToList();

        if (idsNotFound.Count == 0)
        {
            return true;
        }

        validationContext.MessageFormatter
            .AppendArgument("LabelIdsNotFound", string.Join(',', idsNotFound));
        
        return false;
    }
    
    private async Task<bool> AssigneesExistAsync(
        TInput cardInput, 
        IEnumerable<AssigneeId> assigneeIds,
        ValidationContext<TInput> validationContext,
        CancellationToken cancellationToken)
    {
        var ids = assigneeIds
            .Select(x => (UserId)x.Value)
            .ToArray();
        
        var assignees = await _identityService.GetListAsync(ids, cancellationToken);

        var idsNotFound = ids
            .Except(assignees.Select(x => (UserId)x.Id))
            .ToList();

        if (idsNotFound.Count == 0)
        {
            return true;
        }
        
        validationContext.MessageFormatter
            .AppendArgument("AssigneeIdsNotFound", string.Join(',', idsNotFound));
        
        return false;
    }
}
